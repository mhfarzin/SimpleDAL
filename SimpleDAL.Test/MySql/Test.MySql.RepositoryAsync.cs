﻿using System.Threading.Tasks;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestMySqlRepositoryAsync
    {
        [Fact]
        public async Task Insert()
        {
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            await context.Persons.InsertAsync(person);
            Assert.True(person.Id > 0);
        }

        [Fact]
        public async Task Update()
        {
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            await context.Persons.InsertAsync(person);
            person.Age = 42;
            await context.Persons.UpdateAsync(person);
            var changedPerson = await context.Persons.FindAsync(person.Id);
            Assert.Equal(42, changedPerson.Age);
        }

        [Fact]
        public async Task All()
        {
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            await context.Persons.InsertAsync(person);
            var findPersons = await context.Persons.AllAsync();
            Assert.Single(findPersons);
        }

        [Fact]
        public async Task Find()
        {
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            await context.Persons.InsertAsync(person);
            var findPerson = await context.Persons.FindAsync(person.Id);
            Assert.NotNull(findPerson);
        }

        [Fact]
        public async Task Where()
        {
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            await context.Persons.InsertAsync(person);
            var findPerson = await context.Persons.WhereAsync("Id = @id", new { id = person.Id });
            Assert.NotNull(findPerson);
        }

        [Fact]
        public async Task Delete()
        {
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
            context.DataBaseTruncate();
            await context.Persons.InsertAsync(person);
            var findPerson = await context.Persons.FindAsync(person.Id);
            Assert.NotNull(findPerson);
            await context.Persons.DeleteAsync(findPerson.Id);
            var deletePerson = await context.Persons.FindAsync(findPerson.Id);
            Assert.Null(deletePerson);
        }
    }
}
