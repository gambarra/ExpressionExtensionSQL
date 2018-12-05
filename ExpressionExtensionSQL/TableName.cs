using System;

namespace ExpressionExtensionSQL {
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Attribute, IAttributeName {
        public TableName(string name) {
            this.Name = name;
        }
        public string Name { get; }

        public string GetName() {
            return Name;
        }
    }
}
