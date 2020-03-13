using ExpressionExtensionSQL.Tests.Entities;
using System;

namespace ExpressionExtensionSQL.Tests.Configurations
{
    public class DapperFixture : IDisposable
    {
        public TestContext Context { get; }

        public DapperFixture()
        {
            Context = new TestContext();
            Context.Merchant.Add(new Merchant(1, "Merchant 1"));
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
