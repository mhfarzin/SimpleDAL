using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleDal.Test
{
    public class TestMSSqlRepositoryAsync : TestMSSqlBaseClass
    {
        [Fact]
        public async void Insert()
        {

            await TestContext.Persons.InsertAsync(Person);
            Assert.True(Person.Id > 0);
        }

        [Fact]
        public async void Update()
        {
            await TestContext.Persons.InsertAsync(Person);
            Person.Age = 42;
            await TestContext.Persons.UpdateAsync(Person);
            var person = await TestContext.Persons.FindAsync(Person.Id);
            Assert.Equal(42, person.Age);
        }

        [Fact]
        public async void All()
        {
            await TestContext.Persons.InsertAsync(Person);
            await TestContext.Persons.InsertAsync(Person);
            var persons = await TestContext.Persons.AllAsync();
            Assert.Equal(2, persons.Count());
        }

        [Fact]
        public async void Find()
        {
            await TestContext.Persons.InsertAsync(Person);
            var person = await TestContext.Persons.FindAsync(Person.Id);
            Assert.NotNull(person);
        }

        [Fact]
        public async void Where()
        {
            await TestContext.Persons.InsertAsync(Person);
            var person = await TestContext.Persons.WhereAsync("Id = @id", new { id = Person.Id });
            Assert.NotNull(person);
        }

        [Fact]
        public async void Delete()
        {
            await TestContext.Persons.InsertAsync(Person);
            var person = await TestContext.Persons.FindAsync(Person.Id);
            Assert.NotNull(person);
            await TestContext.Persons.DeleteAsync(Person.Id);
            person = await TestContext.Persons.FindAsync(Person.Id);
            Assert.Null(person);
        }
    }
}
