using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL.Test.Contexts
{
    public class TestFluentContext : SimpleDAL.UnitOfWork
    {
        public TestFluentContext(string connectionString) : base(connectionString, SimpleDAL.Provider.SQLite)
        { }

        public SimpleDAL.Repository<FulentPerson> Persons { get; set; }
        public SimpleDAL.Repository<FulentSkill> Skills { get; set; }
        public SimpleDAL.Repository<FulentCourse> Courses { get; set; }
        public SimpleDAL.Repository<FulentCity> Cities { get; set; }

        public void DataBaseTruncate()
        {
            RawNonQuery("DELETE FROM [Persons]; DELETE FROM [Skills]; DELETE FROM [Courses];");
        }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FulentPerson>().Table().Name("Persons");
            modelBuilder.Entity<FulentPerson>().Key(x=>x.Id);
            modelBuilder.Entity<FulentPerson>().Column(x=>x.FullName).Ignore();

            modelBuilder.Entity<FulentCourse>().Table().Name("Courses");
            modelBuilder.Entity<FulentCourse>().Key(x => x.Id).AutoIdentity(false);
            modelBuilder.Entity<FulentCourse>().Binary(x => x.Id);
            modelBuilder.Entity<FulentCourse>().Column(x => x.Name).Name("Title");

            modelBuilder.Entity<FulentSkill>().Table().Name("Skills");
            modelBuilder.Entity<FulentSkill>().Key(x => x.Id);

            modelBuilder.Entity<FulentCity>().Table().Name("Cities");
            modelBuilder.Entity<FulentCity>().Key(x => x.Id);
        }
    }

    public enum FulentGender
    {
        Male = 1,
        Female = 2
    }

    public class FulentPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName => $"{Name} {Family}";
        public int? Age { get; set; }
        public FulentGender Gender { get; set; }
    }

    public class FulentCourse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class FulentSkill
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Title { get; set; }
    }

    public class FulentCity
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
