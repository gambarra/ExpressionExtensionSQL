using ExpressionExtensionSQL.Extensions;
using ExpressionExtensionSQL.Tests.Entities;
using FluentAssertions;
using System;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionExtensionSQL.Test
{
    public class SingleExpressionTest
    {

        [Fact(DisplayName = "SingleExpression - Equal")]
        public void EqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.Name == "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] = @1)");
        }
        [Fact(DisplayName = "SingleExpression - Annotation - Equal")]
        public void EqualWithAnnotationExpression()
        {
            Expression<Func<Order, bool>> expression = x => x.TotalAmount == 1;
            var where = expression.ToSql();
            where.Sql.Should().Be("([tblOrder].[amount] = 1)");
        }


        [Fact(DisplayName = "SingleExpression - NotEqual")]
        public void NotEqualExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.Name != "merchant1";
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[Name] <> @1)");
        }

        [Fact(DisplayName = "SingleExpression - GreaterThan")]
        public void GreaterThanExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount > 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] > 10)");
        }

        [Fact(DisplayName = "SingleExpression - GreaterThanOrEqual")]
        public void GreaterThanOrEqualExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount >= 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] >= 10)");
        }

        [Fact(DisplayName = "SingleExpression - Contains")]
        public void ContainsExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Name.Contains("pedro");
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Name] LIKE @1)");
        }

        [Fact(DisplayName = "SingleExpression - NotContains")]
        public void NotContainsExpression()
        {
            Expression<Func<Product, bool>> expression = x => !x.Name.Contains("pedro");
            var where = expression.ToSql();
            where.Sql.Should().Be("(NOT ([Product].[Name] LIKE @1))");
        }

        [Fact(DisplayName = "SingleExpression - LessThan")]
        public void LessThanExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount < 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] < 10)");
        }

        [Fact(DisplayName = "SingleExpression - LessThanOrEqual")]
        public void LessThanOrEqualExpression()
        {
            Expression<Func<Product, bool>> expression = x => x.Amount <= 10;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Product].[Amount] <= 10)");
        }

        [Fact(DisplayName = "SingleExpression - FluentMap - Equal")]
        public void EqualWithFluentMapExpression()
        {

            Configuration.GetInstance().Entity<Order>().ToTable("tblTeste");
            Configuration.GetInstance().Entity<Order>().Property(p => p.TotalAmount).ToColumn("valor");
            Expression<Func<Order, bool>> expression = x => x.TotalAmount == 1;
            var where = expression.ToSql();
            where.Sql.Should().Be("([tblTeste].[valor] = 1)");
        }

        [Fact(DisplayName = "SingleExpression - IS NULL")]
        public void IsNullExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.DeletedAt == null;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[DeletedAt] IS NULL)");
        }

        [Fact(DisplayName = "SingleExpression - IS NOT NULL")]
        public void IsNotNullExpression()
        {
            Expression<Func<Merchant, bool>> expression = x => x.DeletedAt != null;
            var where = expression.ToSql();
            where.Sql.Should().Be("([Merchant].[DeletedAt] IS NOT NULL)");
        }
    }
}
