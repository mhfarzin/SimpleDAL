using System.Data.SqlClient;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSqlServerException 
    {
        [Fact]
        public void ThrowException()
        {
            using var context = ContextFactory.GetSqlServerContext();
            context.DataBaseTruncate();
            Assert.Throws<SqlException>(() => context.Cities.All());
        }
    }
}
