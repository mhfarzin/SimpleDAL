using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDal.Test
{
    public class TestContext : SimpleDAL.UnitOfWork
    {
        public TestContext(string connectionString) : base(connectionString, SimpleDAL.Provider.SqlServer)
        { }

        public SimpleDAL.Repository<Person> Persons { get; set; }
        public SimpleDAL.Repository<Skill> Skills { get; set; }
        public SimpleDAL.Repository<Courses> Courses { get; set; }
        public SimpleDAL.Repository<City> Cities { get; set; }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    [SimpleDAL.Table("Persons")]
    public class Person
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
    }

    [SimpleDAL.Table("Skills")]
    public class Skill
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Title { get; set; }
    }

    public class Courses
    {
        [SimpleDAL.Key(AutoIdentity = false)]
        public Guid Id { get; set; }
        [SimpleDAL.Column("Title")]
        public string Name { get; set; }
    }

    [SimpleDAL.Table("Cities")]
    public class City
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
