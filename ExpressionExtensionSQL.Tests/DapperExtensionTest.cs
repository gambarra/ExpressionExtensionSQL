using ExpressionExtensionSQL.Dapper;
using ExpressionExtensionSQL.Tests.Configurations;
using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionExtensionSQL.Tests
{
    public class DapperExtensionTest : BaseDapperExtensionTest
    {
        public DapperExtensionTest(DapperFixture fixture) : base(fixture) { }

        [Fact(DisplayName = "DapperExtensionTest - Select inherited type")]
        public void SelectInheritedTypeSuccess()
        {
            Expression<Func<Merchant, bool>> expression = x => x.Id == 1;
            var merchants = Fixture.Context.Database.GetDbConnection()
                .Query("Select * from Merchant {where}", expression: expression);

            merchants.Should().HaveCount(1);
        }
    }
}
