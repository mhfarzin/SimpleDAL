using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSqlServerTransaction
    {
        [Fact]
        public void Transaction_Commit()
        {
            var person = new SqlServerPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SqlServerGender.Male
            };
            using var context = ContextFactory.GetSqlServerContext();
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
            var person = new SqlServerPerson
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = SqlServerGender.Male
            };
            using var context = ContextFactory.GetSqlServerContext();
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
  