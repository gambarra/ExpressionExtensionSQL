using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ExpressionExtensionSQL.Tests.Builders;
using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using Xunit;

namespace ExpressionExtensionSQL.Tests {
    public class PredicateBuilderTest {

        [Fact(DisplayName = "SingleExpression - Equal")]
        public void EqualExpression() {

            var inner = PredicateBuilder.False<Product>();
            inner = inner.Or(p => p.ProductGroupID == 10);
            inner = inner.Or(p => p.ProductGroupID == 11);

            var outer = PredicateBuilder.True<Product>();
            outer = outer.And(p => p.StatusID == 2);
            outer = outer.And(inner);

          
            var where = outer.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] = @1)");
        }
    }
}
