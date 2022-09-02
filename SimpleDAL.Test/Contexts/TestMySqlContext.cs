using MySql.Data.MySqlClient;
using System;

namespace SimpleDAL.Test
{
    public class TestMySqlContext : SimpleDAL.UnitOfWork
    {
        public TestMySqlContext(string connectionString) : base(SimpleDAL.Provider.MySql, new MySqlConnection(connectionString))
        { }

        public SimpleDAL.Repository<MySqlPerson> Persons { get; set; }
        public SimpleDAL.Repository<MySqlSkill> Skills { get; set; }
        public SimpleDAL.Repository<MySqlCourse> Courses { get; set; }
        public SimpleDAL.Repository<MySqlCity> Cities { get; set; }

        public void DataBaseTruncate()
        {
            RawNonQuery("DELETE FROM `persons`; DELETE FROM `skills`; DELETE FROM `courses`;");
        }
    }

    public enum MySqlGender
    {
        Male = 1,
        Female = 2
    }

    [SimpleDAL.Table("Persons")]
    public class MySqlPerson
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        [SimpleDAL.Column(Ignore = true)]
        public string FullName => $"{Name} {Family}";
        public int? Age { get; set; }
        public MySqlGender Gender { get; set; }
    }

    [SimpleDAL.Table("Courses")]
    public class MySqlCourse
    {
        [SimpleDAL.Key(AutoIdentity = false)]
        [SimpleDAL.Binary]
        public Guid Id { get; set; }
        [SimpleDAL.Column("Title")]
        public string Name { get; set; }
    }

    [SimpleDAL.Table("Skills")]
    public class MySqlSkill
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Title { get; set; }
    }

    [SimpleDAL.Table("Cities")]
    public class MySqlCity
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
