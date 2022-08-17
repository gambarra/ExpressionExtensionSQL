using System;
using System.Linq.Expressions;
using ExpressionExtensionSQL.Extensions;
using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using Xunit;

namespace ExpressionExtensionSQL.Tests
{
    public class ConcatenatedExpressionTest
    {
        [Fact(DisplayName = "ConcatenatedExpression - false OR Equal")]
        public void FalseOrEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => false || x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(0=1 OR ([Merchant].[Name] = @1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - true OR Equal")]
        public void TrueOrEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => true || x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(1=1 OR ([Merchant].[Name] = @1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - false AND Equal")]
        public void FalseAndEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => false && x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(0=1 AND ([Merchant].[Name] = @1))");
        }

        [Fact(DisplayName = "ConcatenatedExpression - true AND Equal")]
        public void TrueAndEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => true && x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("(1=1 AND ([Merchant].[Name] = @1))");
        }
    }
}