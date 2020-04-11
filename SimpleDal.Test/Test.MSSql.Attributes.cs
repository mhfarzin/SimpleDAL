using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDal.Test
{
    public class TestMSSqlAttributes : TestMSSqlBaseClass
    {
        [Fact]
        public void NotHavingTableAttributesNotKeyAutoNumber()
        {
            TestContext.Courses.Insert(new Courses { Id = Guid.NewGuid(), Name = "HTML/CSS" });
            var courses = TestContext.Courses.All();
            Assert.Single(courses);
        }
    }
}
