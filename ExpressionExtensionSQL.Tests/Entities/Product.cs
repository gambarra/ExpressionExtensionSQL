namespace ExpressionExtensionSQL.Tests.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int ProductGroupID { get; set; }
        public int StatusID { get; set; }
        public Merchant Merchant { get; set; }
        public bool Active { get; set; }
    }
}