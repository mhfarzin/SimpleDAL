using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;

namespace SimpleDal.Test
{
    public class TestMSSqlException : TestMSSqlBaseClass
    {
        [Fact]
        public void ThrowException()
        {
            Assert.Throws<SqlException>(() => TestContext.Cities.All());
        }
    }
}
