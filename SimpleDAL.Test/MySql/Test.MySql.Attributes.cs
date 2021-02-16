using SimpleDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestMySqlAttributes
    {
        [Fact]
        public void NotHavingTableAttributesNotKeyAutoNumber()
        {
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            context.Courses.Insert(new MySqlCourse { Id = Guid.NewGuid(), Name = "HTML/CSS" });
            var courses = context.Courses.All();
            Assert.Single(courses);
        }
    }
}
