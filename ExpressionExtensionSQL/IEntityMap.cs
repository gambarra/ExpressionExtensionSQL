using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ExpressionExtensionSQL {
    public interface IEntityMap {
        void SetTableName(string tableName);
        Type Type();
        string GetTableName();
        string Name();
    }

    public interface IPropertyMap {
        void SetColumnName(string columnName);
        PropertyInfo Type();
        string GetColumnName();
    }
}
