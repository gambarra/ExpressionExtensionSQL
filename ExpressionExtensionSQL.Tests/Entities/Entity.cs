using System;

namespace ExpressionExtensionSQL.Tests.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}