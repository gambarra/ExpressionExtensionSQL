using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExpressionExtensionSQL {
    public static class WhereBuilder {

        public static WherePart ToSql<T>(this Expression<Func<T, bool>> expression) {
            var i = 1;

            var result= Recurse(ref i, expression.Body, isUnary: true);
            result.Sql = $"{result.Sql}";
            return result;
        }

        private static WherePart Recurse(ref int i, Expression expression, bool isUnary = false, string prefix = null, string postfix = null, bool left =true) {
            if (expression is UnaryExpression) {
                var unary = (UnaryExpression)expression;
                return WherePart.Concat(NodeTypeToString(unary.NodeType), Recurse(ref i, unary.Operand, true));
            }
            if (expression is BinaryExpression) {
                var body = (BinaryExpression)expression;
                return WherePart.Concat(Recurse(ref i, body.Left, left:true), NodeTypeToString(body.NodeType), Recurse(ref i, body.Right,left:false));
            }
            if (expression is ConstantExpression) {
                var constant = (ConstantExpression)expression;
                var value = constant.Value;
                if (value is int) {
                    return WherePart.IsSql(value.ToString());
                }
                if (value is string) {
                    value = prefix + (string)value + postfix;
                }
                if (value is bool && isUnary) {
                    return WherePart.Concat(WherePart.IsParameter(i++, value), "=", WherePart.IsSql("1"));
                }
                return WherePart.IsParameter(i++, value);
            }
            if (expression is MemberExpression) {
                var member = (MemberExpression)expression;

                if (member.Member is PropertyInfo && left) {
                    var property = (PropertyInfo)member.Member;
                    var colName = property.Name;
                    var tableName = property.DeclaringType.Name;
                    if (isUnary && member.Type == typeof(bool)) {
                        return WherePart.Concat(Recurse(ref i, expression), "=", WherePart.IsParameter(i++, true));
                    }
                    return WherePart.IsSql($"[{tableName}].[{ colName }]");
                }
                if (member.Member is FieldInfo || left==false) {
                    var value = GetValue(member);
                    if (value is string) {
                        value = prefix + (string)value + postfix;
                    }
                    return WherePart.IsParameter(i++, value);
                }
                throw new Exception($"Expression does not refer to a property or field: {expression}");
            }
            if (expression is MethodCallExpression) {
                var methodCall = (MethodCallExpression)expression;
                // LIKE queries:
                if (methodCall.Method == typeof(string).GetMethod("Contains", new[] { typeof(string) })) {
                    return WherePart.Concat(Recurse(ref i, methodCall.Object), "LIKE", Recurse(ref i, methodCall.Arguments[0], prefix: "%", postfix: "%"));
                }
                if (methodCall.Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) })) {
                    return WherePart.Concat(Recurse(ref i, methodCall.Object), "LIKE", Recurse(ref i, methodCall.Arguments[0], postfix: "%"));
                }
                if (methodCall.Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) })) {
                    return WherePart.Concat(Recurse(ref i, methodCall.Object), "LIKE", Recurse(ref i, methodCall.Arguments[0], prefix: "%"));
                }
                // IN queries:
                if (methodCall.Method.Name == "Contains") {
                    Expression collection;
                    Expression property;
                    if (methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 2) {
                        collection = methodCall.Arguments[0];
                        property = methodCall.Arguments[1];
                    }
                    else if (!methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 1) {
                        collection = methodCall.Object;
                        property = methodCall.Arguments[0];
                    }
                    else {
                        throw new Exception("Unsupported method call: " + methodCall.Method.Name);
                    }
                    var values = (IEnumerable)GetValue(collection);
                    return WherePart.Concat(Recurse(ref i, property), "IN", WherePart.IsCollection(ref i, values));
                }
                throw new Exception("Unsupported method call: " + methodCall.Method.Name);
            }
            throw new Exception("Unsupported expression: " + expression.GetType().Name);
        }

        private static object GetValue(Expression member) {
            // source: http://stackoverflow.com/a/2616980/291955
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static string NodeTypeToString(ExpressionType nodeType) {
            switch (nodeType) {
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
            return "";//throw new Exception($"Unsupported node type: {nodeType}");
        }
    }
}
