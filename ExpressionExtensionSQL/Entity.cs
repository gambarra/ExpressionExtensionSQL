using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionExtensionSQL
{
    public class Entity<TEntity> : IEntityMap
    {
        private string tableName;

        public Type Type()
        {
            return typeof(TEntity);
        }

        public void SetTableName(string tableName)
        {
            this.tableName = tableName;
        }

        public PropertyEntry<TEntity, TProperty> Property<TProperty>(
            Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            var member = (MemberExpression) propertyExpression.Body;
            return new PropertyEntry<TEntity, TProperty>((PropertyInfo) member.Member);
        }

        public string GetTableName()
        {
            return string.IsNullOrWhiteSpace(tableName) ? typeof(TEntity).Name : tableName;
        }

        public string Name()
        {
            return Type().Name;
        }
    }
}