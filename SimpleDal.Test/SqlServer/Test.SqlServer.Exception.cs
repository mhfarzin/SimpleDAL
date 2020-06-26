﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;

namespace SimpleDal.Test
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
