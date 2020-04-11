using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleDal.Test
{
    public class TestMSSqlTransaction : TestMSSqlBaseClass
    {
        [Fact]
        public void Transaction_Commit()
        {
            using (var transaction = TestContext.BeginTransaction())
            {
                TestContext.Persons.Insert(Person, transaction);
                transaction.Commit();
            }
            var persons = TestContext.Persons.All();
            Assert.Single(persons);
        }

        [Fact]
        public void Transaction_RollBack()
        {
            using (var transaction = TestContext.BeginTransaction())
            {
                TestContext.Persons.Insert(Person, transaction);
                transaction.RollBack();
            }
            var persons = TestContext.Persons.All();
            Assert.Empty(persons);
        }
    }
}
  