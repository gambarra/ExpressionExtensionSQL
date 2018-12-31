using System;
using System.Linq;
using System.Reflection;

namespace ExpressionExtensionSQL {
    public class PropertyEntry<TEntity, TProperty> : IPropertyMap {

        private string columnName;
    
        public PropertyEntry(PropertyInfo propertyInfo) {
            this.propertyInfo = propertyInfo;
        }

        public string GetColumnName() {
            if (string.IsNullOrWhiteSpace(columnName))
                return typeof(TProperty).Name;
            return columnName;
        }

        public void SetColumnName(string columnName) {
            this.columnName = columnName;
        }

        private PropertyInfo propertyInfo;
        public PropertyInfo Type() {

            if (propertyInfo == null) {
                var entity =  typeof(TEntity);
                var property = typeof(TProperty);
                propertyInfo = entity.GetProperties().ToList().FirstOrDefault(p => p.GetType() == property);
            }
            return propertyInfo;
        }
    }
}