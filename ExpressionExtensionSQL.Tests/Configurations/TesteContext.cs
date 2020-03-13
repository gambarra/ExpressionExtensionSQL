using ExpressionExtensionSQL.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpressionExtensionSQL.Tests.Configurations
{
    public sealed class TestContext : DbContext
    {
        public DbSet<Merchant> Merchant { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }

        public TestContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=test.db");
        }
    }
}