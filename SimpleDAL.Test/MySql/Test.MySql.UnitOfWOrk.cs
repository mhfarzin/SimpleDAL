using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleDAL.Test
{
    [Collection("Sequential")]
    public class TestMySqlUnitOfWork
    {
        [Fact]
        public void Set()
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
            context.Set<MySqlPerson>().Insert(person);
            context.Set<MySqlSkill>().Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Set<MySqlSkill>().Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var personsSkills = context.RawQuery<MySqlPersonSkill>(@"SELECT CONCAT(`Persons`.`Name` , ' ' , `Persons`.`Family`) AS FullName, 
                                                                              `Skills`.`Title` AS Skill
                                                                       FROM `Persons` JOIN `Skills` 
                                                                       ON `Persons`.`Id` = `Skills`.`PersonId`;");
            Assert.True(personsSkills.Count() > 0);
        }

        [Fact]
        public void RawQuery()
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
            context.Persons.Insert(person);
            context.Skills.Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Skills.Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var personsSkills = context.RawQuery<MySqlPersonSkill>(@"SELECT CONCAT(`Persons`.`Name` , ' ' , `Persons`.`Family`) AS FullName, 
                                                                              `Skills`.`Title` AS Skill
                                                                       FROM `Persons` JOIN `Skills` 
                                                                       ON `Persons`.`Id` = `Skills`.`PersonId`;");
            Assert.True(personsSkills.Count() > 0);
        }


        [Fact]
        public async Task RawQueryAsync()
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
            await context.Persons.InsertAsync(person);
            await context.Skills.InsertAsync(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            await context.Skills.InsertAsync(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var personsSkills = await context.RawQueryAsync<MySqlPersonSkill>(@"SELECT CONCAT(`Persons`.`Name` , ' ' , `Persons`.`Family`) AS FullName, 
                                                                                          `Skills`.`Title` AS Skill
                                                                                   FROM `Persons` JOIN `Skills` 
                                                                                   ON `Persons`.`Id` = `Skills`.`PersonId`;");
            Assert.True(personsSkills.Count() > 0);
        }

        [Fact]
        public void RawScalatQuery()
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
            context.Persons.Insert(person);
            context.Skills.Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Skills.Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var skillsCount = context.RawScalarQuery<long>("SELECT COUNT(*) FROM `Skills` WHERE `Skills`.`PersonId` = @personId", new { personId = person.Id });
            Assert.Equal(2, skillsCount);
        }

        [Fact]
        public async Task RawScalatQueryAsync()
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
            context.Persons.Insert(person);
            context.Skills.Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "HTML/CSS"
            });
            context.Skills.Insert(new MySqlSkill
            {
                PersonId = person.Id,
                Title = "JavaScript"
            });
            var skillsCount = await context.RawScalarQueryAsync<long>("SELECT COUNT(*) FROM `Skills` WHERE `Skills`.`PersonId` = @personId", new { personId = person.Id });
            Assert.Equal(2, skillsCount);
        }

        [Fact]
        public void RawNonQuery()
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
            context.Persons.Insert(person);
            context.RawNonQuery("UPDATE `Persons` SET `Name` = @name WHERE Id = @id", new { name = "mh", id = person.Id });
            var findPerson = context.Persons.Find(person.Id);
            Assert.Equal("mh", findPerson.Name);
        }

        [Fact]
        public async Task RawNonQueryAsync()
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
            context.Persons.Insert(person);
            await context.RawNonQueryAsync("UPDATE `Persons` SET `Name` = @name WHERE Id = @id", new { name = "mh", id = person.Id });
            var findPerson = context.Persons.Find(person.Id);
            Assert.Equal("mh", findPerson.Name);
        }
    }

    class MySqlPersonSkill
    {
        public string FullName { get; set; }
        public string Skill { get; set; }
    }
}

