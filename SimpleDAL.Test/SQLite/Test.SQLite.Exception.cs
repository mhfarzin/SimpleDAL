using Microsoft.Data.Sqlite;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSQLiteException
    {
        [Fact]
        public void ThrowException()
        {
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            Assert.Throws<SqliteException>(() => context.Cities.All());
        }
    }
}
