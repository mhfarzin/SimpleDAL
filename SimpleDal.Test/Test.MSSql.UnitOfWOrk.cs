using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleDal.Test
{
    public class TestMSSqlUnitOfWork : TestMSSqlBaseClass
    {
        [Fact]
        public void RawQuery()
        {
            TestContext.Persons.Insert(Person);
            TestContext.Skills.Insert(new Skill
            {
                PersonId = Person.Id,
                Title = "HTML/CSS"
            });
            TestContext.Skills.Insert(new Skill
            {
                PersonId = Person.Id,
                Title = "JavaScript"
            });
            var personsSkills = TestContext.RawSqlQuery<PersonSkill>(@"SELECT [Persons].[Name] + ' ' + [Persons].[Family] AS FullName, 
                                                                              [Skills].[Title] AS Skill
                                                                       FROM [Persons] JOIN [Skills] 
                                                                       ON [Persons].[Id] = [Skills].[PersonId];");
            Assert.True(personsSkills.Count() > 0);
        }


        [Fact]
        public async void RawQueryAsync()
        {
            await TestContext.Persons.InsertAsync(Person);
            await TestContext.Skills.InsertAsync(new Skill
            {
                PersonId = Person.Id,
                Title = "HTML/CSS"
            });
            await TestContext.Skills.InsertAsync(new Skill
            {
                PersonId = Person.Id,
                Title = "JavaScript"
            });
            var personsSkills = await TestContext.RawSqlQueryAsync<PersonSkill>(@"SELECT [Persons].[Name] + ' ' + [Persons].[Family] AS FullName, 
                                                                                         [Skills].[Title] AS Skill
                                                                                  FROM [Persons] JOIN [Skills] 
                                                                                  ON [Persons].[Id] = [Skills].[PersonId];");
            Assert.True(personsSkills.Count() > 0);
        }
    }
}

class PersonSkill
{
    public string FullName { get; set; }
    public string Skill { get; set; }
}

