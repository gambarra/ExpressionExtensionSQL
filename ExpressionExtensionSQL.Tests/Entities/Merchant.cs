using System;

namespace ExpressionExtensionSQL.Tests.Entities
{
    public class Merchant : Entity {        
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
