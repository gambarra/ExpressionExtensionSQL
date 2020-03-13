using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExpressionExtensionSQL
{
    public static class WhereBuilder
    {
        public static WherePart ToSql<T>(this Expression<Func<T, bool>> expression)
        {
            var i = 1;

            var result = Recurse<T>(ref i, expression.Body, isUnary: true);
            return result;
        }

        private static WherePart Recurse<T>(ref int i, Expression expression, bool isUnary = false,
            string prefix = null, string postfix = null, bool left = true)
        {
            if (expression is UnaryExpression)
            {
                return UnaryExpressionExtract<T>(ref i, expression);
            }

            if (expression is BinaryExpression)
            {
                return BinaryExpressionExtract<T>(ref i, expression);
            }

            if (expression is ConstantExpression)
            {
                return ConstantExpressionExtract(ref i, expression, isUnary, prefix, postfix, left);
            }

            if (expression is MemberExpression)
            {
                return MemberExpressionExtract<T>(ref i, expression, isUnary, prefix, postfix, left);
            }

            if (expression is MethodCallExpression)
            {
                return MethodCallExpressionExtract<T>(ref i, expression);
            }

            if (expression is InvocationExpression)
            {
                return InvocationExpressionExtract<T>(ref i, expression, left);
            }

            throw new Exception("Unsupported expression: " + expression.GetType().Name);
        }


        private static WherePart InvocationExpressionExtract<T>(ref int i, Expression expression, bool left)
        {
            var methodCall = (InvocationExpression) expression;

            return Recurse<T>(ref i, ((Expression<Func<T, bool>>) methodCall.Expression).Body, left: left);
        }

        private static WherePart MethodCallExpressionExtract<T>(ref int i, Expression expression)
        {
            var methodCall = (MethodCallExpression) expression;
            // LIKE queries:
            if (methodCall.Method == typeof(string).GetMethod("Contains", new[] {typeof(string)}))
            {
                return WherePart.Concat(Recurse<T>(ref i, methodCall.Object), "LIKE",
                    Recurse<T>(ref i, methodCall.Arguments[0], prefix: "%", postfix: "%"));
            }

            if (methodCall.Method == typeof(string).GetMethod("StartsWith", new[] {typeof(string)}))
            {
                return WherePart.Concat(Recurse<T>(ref i, methodCall.Object), "LIKE",
                    Recurse<T>(ref i, methodCall.Arguments[0], postfix: "%"));
            }

            if (methodCall.Method == typeof(string).GetMethod("EndsWith", new[] {typeof(string)}))
            {
                return WherePart.Concat(Recurse<T>(ref i, methodCall.Object), "LIKE",
                    Recurse<T>(ref i, methodCall.Arguments[0], prefix: "%"));
            }

            if (methodCall.Method == typeof(string).GetMethod("Equals", new[] {typeof(string)}))
            {
                return WherePart.Concat(Recurse<T>(ref i, methodCall.Object), "=",
                    Recurse<T>(ref i, methodCall.Arguments[0], left: false));
            }

            // IN queries:
            if (methodCall.Method.Name == "Contains")
            {
                Expression collection;
                Expression property;
                if (methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 2)
                {
                    collection = methodCall.Arguments[0];
                    property = methodCall.Arguments[1];
                }
                else if (!methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 1)
                {
                    collection = methodCall.Object;
                    property = methodCall.Arguments[0];
                }
                else
                {
                    throw new Exception("Unsupported method call: " + methodCall.Method.Name);
                }

                var values = (IEnumerable) GetValue(collection);
                return WherePart.Concat(Recurse<T>(ref i, property), "IN", WherePart.IsCollection(ref i, values));
            }

            throw new Exception("Unsupported method call: " + methodCall.Method.Name);
        }

        private static WherePart MemberExpressionExtract<T>(ref int i, Expression expression, bool isUnary,
            string prefix, string postfix, bool left)
        {
            var member = (MemberExpression) expression;

            if (isUnary && member.Type == typeof(bool))
            {
                return WherePart.Concat(Recurse<T>(ref i, expression), "=", WherePart.IsSql("1"));
            }

            if (member.Member is PropertyInfo)
            {
                var property = (PropertyInfo) member.Member;
                if (left)
                {
                    var colName = GetName<ColumnName>(property);
                    var tableName = GetName<TableName>(property.DeclaringType.IsAbstract
                        ? ((ParameterExpression) member.Expression).Type
                        : property.DeclaringType);
                    return WherePart.IsSql($"[{tableName}].[{colName}]");
                }
                else if (left == false && property.PropertyType == typeof(bool))
                {
                    var colName = GetName<ColumnName>(property);
                    var tableName = GetName<TableName>(property.DeclaringType.IsAbstract
                        ? ((ParameterExpression) member.Expression).Type
                        : property.DeclaringType);

                    return WherePart.IsSql($"[{tableName}].[{colName}]=1");
                }
            }


            if (member.Member is FieldInfo || left == false)
            {
                var value = GetValue(member);
                if (value is string)
                {
                    value = prefix + (string) value + postfix;
                }

                return WherePart.IsParameter(i++, value);
            }

            throw new Exception($"Expression does not refer to a property or field: {expression}");
        }

        private static string GetName<T>(PropertyInfo pi) where T : IAttributeName
        {
            if (Configuration.GetInstance().Properties() != null)
            {
                var result = Configuration.GetInstance().Properties().FirstOrDefault(p => p.Type() == pi);
                if (result != null)
                    return result.GetColumnName();
            }

            var attributes = pi.GetCustomAttributes(typeof(T), false).AsList();
            if (attributes.Count != 1) return pi.Name;

            var attributeName = (T) attributes[0];
            return attributeName.GetName();
        }

        private static string GetName<T>(Type type) where T : IAttributeName
        {
            if (Configuration.GetInstance().Entities() != null)
            {
                var result = Configuration.GetInstance()
                    .Entities()
                    .FirstOrDefault(p => p.Name().Equals(type.Name, StringComparison.CurrentCultureIgnoreCase));
                if (result != null)
                    return result.GetTableName();
            }

            var attributes = type.GetCustomAttributes(typeof(T), false).AsList();
            if (attributes.Count != 1) return type.Name;

            var attributeName = (T) attributes[0];
            return attributeName.GetName();
        }

        private static WherePart ConstantExpressionExtract(ref int i, Expression expression, bool isUnary,
            string prefix, string postfix, bool left)
        {
            var constant = (ConstantExpression) expression;
            var value = constant.Value;

            switch (value)
            {
                case null:
                    return WherePart.IsSql("NULL");
                case int _:
                    return WherePart.IsSql(value.ToString());
                case string text:
                    value = prefix + text + postfix;
                    break;
            }

            if (!(value is bool) || isUnary) return WherePart.IsParameter(i++, value);

            var result = ((bool) value) ? "1" : "0";
            if (left)
                result = result.Equals("1") ? "1=1" : "0=0";
            return WherePart.IsSql(result);
        }

        private static WherePart BinaryExpressionExtract<T>(ref int i, Expression expression)
        {
            var body = (BinaryExpression) expression;
            return WherePart.Concat(Recurse<T>(ref i, body.Left, left: true), NodeTypeToString(body.NodeType),
                Recurse<T>(ref i, body.Right, left: false));
        }

        private static WherePart UnaryExpressionExtract<T>(ref int i, Expression expression)
        {
            var unary = (UnaryExpression) expression;
            return WherePart.Concat(NodeTypeToString(unary.NodeType), Recurse<T>(ref i, unary.Operand, true));
        }

        private static object GetValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static string NodeTypeToString(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                    return "AND";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Negate:
                    return "-";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return "OR";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Subtract:
                    return "-";
            }

            return string.Empty;
        }

        public static List<T> AsList<T>(this IEnumerable<T> source) =>
            (source == null || source is List<T>) ? (List<T>) source : source.ToList();
    }
}