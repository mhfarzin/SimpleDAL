using SimpleDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSQLiteAttributes
    {
        [Fact]
        public void NotHavingTableAttributesNotKeyAutoNumber()
        {
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Courses.Insert(new SQLiteCourse { Id = Guid.NewGuid(), Name = "HTML/CSS" });
            var courses = context.Courses.All();
            Assert.Single(courses);
        }
    }
}
