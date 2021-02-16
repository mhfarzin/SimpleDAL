using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSQLiteTransaction
    {
        [Fact]
        public void Transaction_Commit()
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
            using (var transaction = context.BeginTransaction())
            {
                context.Persons.Insert(person, transaction);
                transaction.Commit();
            }
            var persons = context.Persons.All();
            Assert.Single(persons);
        }

        [Fact]
        public void Transaction_RollBack()
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
            using (var transaction = context.BeginTransaction())
            {
                context.Persons.Insert(person, transaction);
                transaction.RollBack();
            }
            var persons = context.Persons.All();
            Assert.Empty(persons);
        }
    }
}
  