﻿using MySql.Data.MySqlClient;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestMySqlException
    {
        [Fact]
        public void ThrowException()
        {
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            Assert.Throws<MySqlException>(() => context.Cities.All());
        }
    }
}
