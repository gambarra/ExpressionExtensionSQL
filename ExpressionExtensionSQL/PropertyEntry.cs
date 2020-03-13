using System.Linq;
using System.Reflection;

namespace ExpressionExtensionSQL
{
    public class PropertyEntry<TEntity, TProperty> : IPropertyMap
    {
        private string columnName;
        private PropertyInfo propertyInfo;

        public PropertyEntry(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public string GetColumnName()
        {
            return string.IsNullOrWhiteSpace(columnName) ? typeof(TProperty).Name : columnName;
        }

        public void SetColumnName(string columnName)
        {
            this.columnName = columnName;
        }

        public PropertyInfo Type()
        {
            if (propertyInfo != null)
            {
                return propertyInfo;
            }

            var entity = typeof(TEntity);
            var property = typeof(TProperty);
            propertyInfo = entity.GetProperties().ToList().FirstOrDefault(p => p.GetType() == property);
            return propertyInfo;
        }
    }
}