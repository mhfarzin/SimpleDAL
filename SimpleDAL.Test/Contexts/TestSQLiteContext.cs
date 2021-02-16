using System;

namespace SimpleDAL.Test
{
    public class TestSQLiteContext : SimpleDAL.UnitOfWork
    {
        public TestSQLiteContext(string connectionString) : base(connectionString, SimpleDAL.Provider.SQLite)
        { }

        public SimpleDAL.Repository<SQLitePerson> Persons { get; set; }
        public SimpleDAL.Repository<SQLiteSkill> Skills { get; set; }
        public SimpleDAL.Repository<SQLiteCourse> Courses { get; set; }
        public SimpleDAL.Repository<SQLiteCity> Cities { get; set; }

        public void DataBaseTruncate()
        {
            RawNonQuery("DELETE FROM Persons; DELETE FROM Skills; DELETE FROM Courses;");
        }
    }

    public enum SQLiteGender
    {
        Male = 1,
        Female = 2
    }

    [SimpleDAL.Table("Persons")]
    public class SQLitePerson
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        [SimpleDAL.Column(Ignore = true)]
        public string FullName => $"{Name} {Family}";
        public int? Age { get; set; }
        public SQLiteGender Gender { get; set; }
    }

    [SimpleDAL.Table("Courses")]
    public class SQLiteCourse
    {
        [SimpleDAL.Key(AutoIdentity = false)]
        [SimpleDAL.Binary]
        public Guid Id { get; set; }
        [SimpleDAL.Column("Title")]
        public string Name { get; set; }
    }

    [SimpleDAL.Table("Skills")]
    public class SQLiteSkill
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Title { get; set; }
    }

    [SimpleDAL.Table("Cities")]
    public class SQLiteCity
    {
        [SimpleDAL.Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
