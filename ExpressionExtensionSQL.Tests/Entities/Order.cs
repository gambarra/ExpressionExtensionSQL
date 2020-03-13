using System;
using System.Collections.Generic;

namespace ExpressionExtensionSQL.Tests.Entities
{
    [TableName("tblOrder")]
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [ColumnName("amount")] public int TotalAmount { get; set; }
        public IList<Item> items { get; set; }
    }

    public class Item : Entity
    {
        public Product Product { get; private set; }
        public int Count { get; private set; }
        public int Total { get; set; }
    }
}