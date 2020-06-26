using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleDal.Test
{
    [Collection("Sequential")]
    public class TestMySqlTransaction
    {
        [Fact]
        public void Transaction_Commit()
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
            var person = new MySqlPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = MySqlGender.Male
            };
            using var context = ContextFactory.GetMySqlContext();
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
  