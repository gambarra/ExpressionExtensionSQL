using Xunit;

namespace ExpressionExtensionSQL.Tests.Configurations
{
    public class BaseDapperExtensionTest : IClassFixture<DapperFixture>
    {
        protected DapperFixture Fixture { get; private set; }
        public BaseDapperExtensionTest(DapperFixture fixture)
        {
            Fixture = fixture;
        }
    }
}
