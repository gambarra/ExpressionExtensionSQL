using System;
using System.Collections.Generic;
using System.Linq;
namespace ExpressionExtensionSQL
{

    public class Configuration
    {
        private static IDictionary<string, string> ColumnsMap;
 
        public static bool SetColumnMap<T>(string tableName)
        {
            if (ColumnsMap == null)
                ColumnsMap = new Dictionary<string, string>();

            if (ColumnsMap.Keys.Any(p => p.Equals(typeof(T).Name, StringComparison.CurrentCultureIgnoreCase)))
                return false;

            if (ColumnsMap.Values.Any(p => p.Equals(tableName, StringComparison.CurrentCultureIgnoreCase)))
                return false;

            ColumnsMap.Add(new KeyValuePair<string, string>(typeof(T).Name, tableName));

            return true;
        }

        public static IDictionary<string, string> GetColumnsMap() => ColumnsMap;

    }
}
