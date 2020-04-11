# SimpleDAL
SimpleDAL is a lightweight library that allows you to quickly create a data access layer.

## Installation
Add a reference to the library from [https://www.nuget.org/packages/SimpleDAL/](https://www.nuget.org/packages/SimpleDAL/)

## Getting Start
First create a class for each of your tables.
```
public enum Gender
{
    Male = 1,
    Female = 2
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public int? Age { get; set; }
    public Gender Gender { get; set; }
}

public class Skill
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string Title { get; set; }
}
```

Next you need to create a context class for yourself. This class must have inherited from SimpleDAL UnitOfWork class. We also consider a SimpleDAL Repository class as property for each entity.
```
using SimpleDal;

public class MyContext : UnitOfWork
{
    public MyContext() : base("your sql server connectionString ...", Provider.SqlServer)
    { }

    public Repository<Person> Persons { get; set; }
    public Repository<Skill> Skills { get; set; }
}
```

Now you can connect to the database by making a instance from MyContext class.
```
var context = new MyContext();

var person = new Person { ... } ;
context.Persons.Insert(person);

var persons = context.Persons.All();
```

# Documet


## Attribute

### Table
It is used when you want the table name in the database not to be the same as the class name

```
using SimpleDal;

[Table("Persons")]
public class Person
{
    ...
}
```

### Key
The table key is used to set key field. AutoIdentity property can also be set to value. If you do not use this attribute, default Id field is a key. The default value of AutoIdentity is also equal to true.

```
using SimpleDal;

public class Person
{
    [Key]
    public int Id { get; set; }
    ...
}
```

```
using SimpleDal;

public class Person
{
    [Key(AutoIdentity = false)]
    public Guid Id { get; set; }
    ...
}
```

### Column
We use this attribute when the database column name is different from property name

```
using SimpleDal;

public class Person
{
    [Column("firstname")]
    public string Name { get; set; }
    ...
}
```

## Repository
For each table we can have a repository in the contaxt

All of the above methods also have a async version 
for example you can use AllAsync Instead All

each repository methods include:
### All And AllAsync
```
var persons = context.Persons.All();
```
### Find And FindAsync
```
var person = context.Persons.Find(Person.Id);
```
### Where And WhereAsync
```
var person = context.Persons.Where("Id = @id", new { id = Person.Id });
```
### Insert And InsertAsync
```
var person = new Person
{
    Name = ...
};
context.Persons.Insert(Person); //key field if is AutoIdentity gets value after inserting
```
### Update And UpdateAsync
```
var person = context.Persons.Find(Person.Id);
person.name = ...
context.Persons.Update(person);
```
### Delete And DeleteAsync
```
context.Persons.Delete(PersonId);
```

## Unit OF Work
UnitOfWork addition to the Repository, has three other methods. note that the inherited class from UnitOfWork is not thread safe. Therefore, it must be added as a scoped  in IoC container
### RawSqlQuery And RawSqlQueryAsync
You can use this method when you want to make a special query
```
var personsSkills = context.RawSqlQuery<PersonSkill>(
    @"SELECT [Persons].[Name] + ' ' + [Persons].[Family] AS FullName, [Skills].[Title] AS Skill
      FROM [Persons] JOIN [Skills] ON [Persons].[Id] = [Skills].[PersonId];");
      
...
      
class PersonSkill
{
    public string FullName { get; set; }
    public string Skill { get; set; }
}
```
### BeginTransaction
If you want to use a transaction, you can do this using this method.
```
using (var transaction = context.BeginTransaction())
{
    context.Persons.Insert(Person1, transaction);
    context.Persons.Insert(Person2, transaction);
    transaction.Commit();
}
var persons = context.Persons.All();
```
