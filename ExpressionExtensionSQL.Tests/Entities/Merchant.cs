using System;

namespace ExpressionExtensionSQL.Tests.Entities
{
    public class Merchant : Entity
    {
        private Merchant()
        {
        }

        public Merchant(int id, string name)
        {
            Id = id;
            Name = name;
            CreatedAt = DateTime.Now;
        }

        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public DateTime? DeletedAt { get; private set; }
    }
}