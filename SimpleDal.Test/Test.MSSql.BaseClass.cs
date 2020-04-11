using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SimpleDal.Test
{
    public class TestMSSqlBaseClass : IDisposable
    {
        public const string ConnectionString = @"Server =.\SQLEXPRESS; Database=SimpleDAL;Trusted_Connection=True;";
        protected TestContext TestContext;
        protected Person Person;

        public TestMSSqlBaseClass()
        {
            ClearDataBaseTables();
            TestContext = new TestContext(ConnectionString);
            Person = new Person
            {
                Name = "MohammadHasan",
                Family = "Farzin",
                Age = 32,
                Gender = Gender.Male
            };
        }

        public void Dispose()
        {
            TestContext = null;
            ClearDataBaseTables();
            GC.SuppressFinalize(this);
        }

        private void ClearDataBaseTables()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            using var command = new SqlCommand("DELETE FROM [Persons]; DELETE FROM [Skills]; DELETE FROM [Courses];", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
