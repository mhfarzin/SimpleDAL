﻿using SimpleDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSQLiteRepository
    {
        [Fact]
        public void Insert()
        {
            var person = new SQLitePerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SQLiteGender.Male
            };
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Persons.Insert(person);
            Assert.True(person.Id > 0);
        }

        [Fact]
        public void Update()
        {
            var person = new SQLitePerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SQLiteGender.Male
            };
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Persons.Insert(person);
            person.Age = 42;
            context.Persons.Update(person);
            var changedPerson = context.Persons.Find(person.Id);
            Assert.Equal(42, changedPerson.Age);
        }

        [Fact]
        public void All()
        {
            var person = new SQLitePerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SQLiteGender.Male
            };
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Persons.Insert(person);
            var persons = context.Persons.All();
            Assert.Single(persons);
        }

        [Fact]
        public void Find()
        {
            var person = new SQLitePerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SQLiteGender.Male
            };
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Persons.Insert(person);
            var findPerson = context.Persons.Find(person.Id);
            Assert.NotNull(findPerson);
        }

        [Fact]
        public void Where()
        {
            var person = new SQLitePerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SQLiteGender.Male
            };
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Persons.Insert(person);
            var findPerson = context.Persons.Where("Id = @id", new { id = person.Id });
            Assert.NotNull(findPerson);
        }

        [Fact]
        public void Delete()
        {
            var person = new SQLitePerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SQLiteGender.Male
            };
            using var context = ContextFactory.GetSqlLiteContext();
            context.DataBaseTruncate();
            context.Persons.Insert(person);
            var findPerson = context.Persons.Find(person.Id);
            Assert.NotNull(person);
            context.Persons.Delete(person.Id);
            var deletePerson = context.Persons.Find(person.Id);
            Assert.Null(deletePerson);
        }
    }
}
