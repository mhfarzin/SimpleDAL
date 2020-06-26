using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SimpleDal.Test
{
    public class TestSqlServerContext : SimpleDAL.UnitOfWork
    {
        public TestSqlServerContext(string connectionString) : base(connectionString, SimpleDAL.Provider.SqlServer)
        { }

        public SimpleDAL.Repository<SqlServerPerson> Persons { get; set; }
        public SimpleDAL.Repository<SqlServerSkill> Skills { get; set; }
        public SimpleDAL.Repository<SqlServerCourse> Courses { get; set; }
        public SimpleDAL.Repository<SqlServerCity> Cities { get; set; }

        public void DataBaseTruncate()
        {
            RawNonQuery("DELETE FROM [Persons]; DELETE FROM [Skills]; DELETE FROM [Courses];");
        }
    }

    public enum SqlServerGender
    {
        Male = 1,
        Female = 2
    }

    [SimpleDAL.Table("Persons")]
    public class SqlServerPerson
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        [SimpleDAL.Column(Ignore = true)]
        public string FullName => $"{Name} {Family}";
        public int? Age { get; set; }
        public SqlServerGender Gender { get; set; }
    }

    [SimpleDAL.Table("Courses")]
    public class SqlServerCourse
    {
        [SimpleDAL.Key(AutoIdentity = false)]
        public Guid Id { get; set; }
        [SimpleDAL.Column("Title")]
        public string Name { get; set; }
    }

    [SimpleDAL.Table("Skills")]
    public class SqlServerSkill
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Title { get; set; }
    }

    [SimpleDAL.Table("Cities")]
    public class SqlServerCity
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
