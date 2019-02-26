using System;

namespace ExpressionExtensionSQL.Tests.Entities {
    public abstract class Entity {
        public int Id { get; set; }
    }
    public class Merchant : Entity {
        
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsEnabled { get; set; }
    }
}
