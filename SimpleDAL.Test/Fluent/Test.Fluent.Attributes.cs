using SimpleDAL;
using SimpleDAL.Test.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestFluentAttributes
    {
        [Fact]
        public void NotHavingTableAttributesNotKeyAutoNumber()
        {
            using var context = ContextFactory.GetTestFluentContext();
            context.DataBaseTruncate();
            context.Courses.Insert(new FulentCourse { Id = Guid.NewGuid(), Name = "HTML/CSS" });
            var courses = context.Courses.All();
            Assert.Single(courses);
        }
    }
}
