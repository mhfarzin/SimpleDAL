using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestSQLiteUnitOfWork
    {
        [Fact]
        public void Set()
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
            context.Set<SQLitePerson>().Insert(person);
            context.Set<SQLiteSkill>().Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Set<SQLiteSkill>().Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var personsSkills = context.RawQuery<SQLitePersonSkill>(@"SELECT [Persons].[Name] + ' ' + [Persons].[Family] AS FullName, 
                                                                              [Skills].[Title] AS Skill
                                                                       FROM [Persons] JOIN [Skills] 
                                                                       ON [Persons].[Id] = [Skills].[PersonId];");
            Assert.True(personsSkills.Count() > 0);
        }

        [Fact]
        public void RawQuery()
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
            context.Persons.Insert(person);
            context.Skills.Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Skills.Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var personsSkills = context.RawQuery<SQLitePersonSkill>(@"SELECT [Persons].[Name] + ' ' + [Persons].[Family] AS FullName, 
                                                                              [Skills].[Title] AS Skill
                                                                       FROM [Persons] JOIN [Skills] 
                                                                       ON [Persons].[Id] = [Skills].[PersonId];");
            Assert.True(personsSkills.Count() > 0);
        }


        [Fact]
        public async Task RawQueryAsync()
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
            await context.Persons.InsertAsync(person);
            await context.Skills.InsertAsync(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            await context.Skills.InsertAsync(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var personsSkills = await context.RawQueryAsync<SQLitePersonSkill>(@"SELECT [Persons].[Name] + ' ' + [Persons].[Family] AS FullName, 
                                                                                         [Skills].[Title] AS Skill
                                                                                  FROM [Persons] JOIN [Skills] 
                                                                                  ON [Persons].[Id] = [Skills].[PersonId];");
            Assert.True(personsSkills.Count() > 0);
        }

        [Fact]
        public void RawScalatQuery()
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
            context.Persons.Insert(person);
            context.Skills.Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Skills.Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var skillsCount = context.RawScalarQuery<long>("SELECT COUNT(*) FROM [Skills] WHERE [Skills].[PersonId] = @personId", new { personId = person.Id });
            Assert.Equal(2, skillsCount);
        }

        [Fact]
        public async Task RawScalatQueryAsync()
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
            context.Persons.Insert(person);
            context.Skills.Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Skills.Insert(new SQLiteSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var skillsCount = await context.RawScalarQueryAsync<long>("SELECT COUNT(*) FROM [Skills] WHERE [Skills].[PersonId] = @personId", new { personId = person.Id });
            Assert.Equal(2, skillsCount);
        }

        [Fact]
        public void RawNonQuery()
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
            context.Persons.Insert(person);
            context.RawNonQuery("UPDATE [Persons] SET [Name] = @name WHERE Id = @id", new { name = "mh", id = person.Id });
            var findPerson = context.Persons.Find(person.Id);
            Assert.Equal("mh", findPerson.Name);
        }

        [Fact]
        public async Task RawNonQueryAsync()
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
            context.Persons.Insert(person);
            await context.RawNonQueryAsync("UPDATE [Persons] SET [Name] = @name WHERE Id = @id", new { name = "mh", id = person.Id });
            var findPerson = context.Persons.Find(person.Id);
            Assert.Equal("mh", findPerson.Name);
        }
    }

    class SQLitePersonSkill
    {
        public string FullName { get; set; }
        public string Skill { get; set; }
    }
}

