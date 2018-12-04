using ExpressionExtensionSQL.Test.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace ExpressionExtensionSQL.Test {
    public class SingleExpressionTest {

        [Fact]
        public void EqualExpressionForInt() {

            Expression<Func<Merchant, bool>> expression = x => x.Name == "merchant1";
            var where = expression.ToSql();

            where.Sql.Should().Be("");

        }
    }
}
