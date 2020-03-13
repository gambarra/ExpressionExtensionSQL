using System.Data;
using FluentAssertions;
using Xunit;

namespace ExpressionExtensionSQL.Tests
{
    public class ParameterTests
    {
        [Fact(DisplayName = "DapperExtensionTest - Use AnsiString type for String")]
        public void UseAnsiStringTypeForString()
        {
            var parameter = new Parameter("ExternalId", "parameter");
            parameter.Type.Should().Be(DbType.AnsiString);
        }
    }
}