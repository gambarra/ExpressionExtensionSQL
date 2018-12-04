using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionExtensionSQL.Test.Entities {
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public Merchant Merchant { get; set; }
    }
}
