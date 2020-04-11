using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDal.Test
{
    public class TestMSSqlRepository : TestMSSqlBaseClass
    {
        [Fact]
        public void Insert()
        {
            TestContext.Persons.Insert(Person);
            Assert.True(Person.Id > 0);
        }

        [Fact]
        public void Update()
        {
            TestContext.Persons.Insert(Person);
            Person.Age = 42;
            TestContext.Persons.Update(Person);
            var person = TestContext.Persons.Find(Person.Id);
            Assert.Equal(42, person.Age);
        }

        [Fact]
        public void All()
        {
            TestContext.Persons.Insert(Person);
            TestContext.Persons.Insert(Person);
            var persons = TestContext.Persons.All();
            Assert.Equal(2, persons.Count());
        }

        [Fact]
        public void Find()
        {
            TestContext.Persons.Insert(Person);
            var person = TestContext.Persons.Find(Person.Id);
            Assert.NotNull(person);
        }

        [Fact]
        public void Where()
        {
            TestContext.Persons.Insert(Person);
            var person = TestContext.Persons.Where("Id = @id", new { id = Person.Id });
            Assert.NotNull(person);
        }

        [Fact]
        public void Delete()
        {
            TestContext.Persons.Insert(Person);
            var person = TestContext.Persons.Find(Person.Id);
            Assert.NotNull(person);
            TestContext.Persons.Delete(Person.Id);
            person = TestContext.Persons.Find(Person.Id);
            Assert.Null(person);
        }
    }
}
