using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ExpressionExtensionSQL.Test {
    public class SingleExpressionTest {

        [Fact(DisplayName = "SingleExpression - Equal")]
        public void EqualExpression() {
            Expression<Func<Merchant, bool>> expression = x => x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] = @1)");
        }

        [Fact(DisplayName = "SingleExpression - GreaterThan")]
        public void GreaterThanExpression() {
            Expression<Func<Product, bool>> expression = x => x.Amount > 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] > 10)");
        }

        [Fact(DisplayName = "SingleExpression - GreaterThanOrEqual")]
        public void GreaterThanOrEqualExpression() {
            Expression<Func<Product, bool>> expression = x => x.Amount >= 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] >= 10)");
        }

        [Fact(DisplayName = "SingleExpression - Contains")]
        public void ContainsExpression() {
            Expression<Func<Product, bool>> expression = x => x.Name.Contains("pedro");
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Name] LIKE @1)");
        }

        [Fact(DisplayName = "SingleExpression - NotContains")]
        public void NotContainsExpression() {
            Expression<Func<Product, bool>> expression = x => !x.Name.Contains("pedro");
            var where = expression.ToSql();
            where.Sql.Should().Be("(NOT ([Product].[Name] LIKE @1))");
        }

    }
}
