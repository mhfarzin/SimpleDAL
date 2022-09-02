using System;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSqlServerAttributes
    {
        [Fact]
        public void NotHavingTableAttributesNotKeyAutoNumber()
        {
            using var context = ContextFactory.GetSqlServerContext();
            context.DataBaseTruncate();
            context.Courses.Insert(new SqlServerCourse { Id = Guid.NewGuid(), Name = "HTML/CSS" });
            var courses = context.Courses.All();
            Assert.Single(courses);
        }
    }
}
