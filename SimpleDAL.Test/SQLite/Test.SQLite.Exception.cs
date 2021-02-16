using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
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
            Assert.Throws<SQLiteException>(() => context.Cities.All());
        }
    }
}
