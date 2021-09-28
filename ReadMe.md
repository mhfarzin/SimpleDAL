# SimpleDAL
SimpleDAL is a lightweight library that allows you to quickly create a data access layer. simpledal supports sql server, mysql and sqlite.

![](https://raw.githubusercontent.com/mhfarzin/SimpleDAL/master/SimpleDAL-Logo.png)

## Installation
Add a reference to the library from [https://www.nuget.org/packages/SimpleDAL/](https://www.nuget.org/packages/SimpleDAL/)

## Getting Start
First create a class for each of your tables.  
If the class name and table name are not the same, you must use the [table](https://github.com/mhfarzin/SimpleDAL#table) Attribute.

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

Also, if you want to connect to a mysql database, just set the second parameter to the value of Provider.MySql
```
using SimpleDal;

public class MyContext : UnitOfWork
{
    public MyContext() : base("your mysql connectionString ...", Provider.MySql)
    { }

    public Repository<Person> Persons { get; set; }
    public Repository<Skill> Skills { get; set; }
}
```

And for sqlite database, just set the second parameter to the value of Provider.SQLite
```
using SimpleDal;

public class MyContext : UnitOfWork
{
    public MyContext() : base("your sqlite connectionString ...", Provider.SQLite)
    { }

    public Repository<Person> Persons { get; set; }
    public Repository<Skill> Skills { get; set; }
}
```

Finally you can connect to the database by making a instance from MyContext class.
```
var context = new MyContext();

var person = new Person { ... } ;
context.Persons.Insert(person);

var persons = context.Persons.All();
```

# Documet


## Configuration Models
You can config your models in two ways
1) use of attributes
2) use of flow

below you can see both forms of each config. note that you only need to use one of these two forms.

### Table
It is used when you want the table name in the database not to be the same as the class name

if you want use as attribute
```
using SimpleDal;

[Table("Persons")]
public class Person
{
    ...
}
```

if you want use as fluent
```
using SimpleDal;

public class Person
{
    ...
}
public class MyContext : UnitOfWork {
	...
	public override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Person>().Table().Name("Persons");
	}
}
```

### Key
The table key is used to set key field. AutoIdentity property can also be set to value. If you do not use this attribute, default Id field is a key. The default value of AutoIdentity is also equal to true.

if you want use as attribute
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

if you want use as fluent
```
using SimpleDal;

public class Person
{
	public Guid Id { get; set; }
    ...
}
public class MyContext : UnitOfWork {
	...
	public override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Person>().Key(x => x.Id).AutoIdentity(false);
	}
}
```


### Column
We use this attribute when the database column name is different from property name

if you want use as attribute
```
using SimpleDal;

public class Person
{
    [Column("firstname")]
    public string Name { get; set; }
    ...
}
```

You can also do this when you don't need to store a property in the database.
```
using SimpleDal;

public class Person
{
	[Column("firstname")]
    public string Name { get; set; }
    public string Family { get; set; }
    [SimpleDAL.Column(Ignore = true)]
    public string FullName => $"{Name} {Family}";
    ...
}
```
if you want use as fluent
```
using SimpleDal;

public class Person
{
    public string Name { get; set; }
    public string Family { get; set; }
    public string FullName => $"{Name} {Family}";
}
public class MyContext : UnitOfWork {
	...
	public override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Person>().Column(x=>x.Name).Name("firstname");
		modelBuilder.Entity<Person>().Column(x=>x.FullName).Ignore();
	}
}
```


### Binary
To save a field as a binary, you can easily define its property from byte[].
```
public class Images
{
    ...
    public byte[] File { get; set; }
    ...
}
```
But you may not want the property be defined as byte[] but saved as a byte[]. An example of this is when your database is mysql or sqlite and you want to save the Guid field (in the mysql and sqlite, the Guid Can be stored as byte[]). with this feature, your property will automatically be converted to byte[] at the time of storage, and at the time of reading, it will be mapped again.

if you want use as attribute
```
using SimpleDal;

[SimpleDAL.Table("Courses")]
public class Course
{
    [SimpleDAL.Key(AutoIdentity = false)]
    [SimpleDAL.Binary]
    public Guid Id { get; set; }
    ...
}
```
if you want use as fluent
```
using SimpleDal;

public class Course
{
	public Guid Id { get; set; }
    ...
}
public class MyContext : UnitOfWork {
	...
	public override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Course>().Table().Name("Courses");
		modelBuilder.Entity<Course>().Key(x => x.Id).AutoIdentity(false);
		modelBuilder.Entity<Course>().Binary(x => x.Id);
	}
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
UnitOfWork addition to the Repository, has some other methods. note that the inherited class from UnitOfWork is not thread safe. Therefore, it must be added as a scoped  in IoC container
### Set
With this command, you can get specify repository by generic parameter class. the following two commands are equal.
```
context.Persons.Insert(new Person 
{
    Name = ...
}); 
```
```
context.Set<Person>().Insert(new Person 
{
    Name = ...
}); 
```
      
### RawQuery And RawQueryAsync
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
### RawScalarQuery And RawScalarQueryAsync
You can use this method when you want to make a special scalar query
```
var personsCounts = context.RawSqlQuery<int>("SELECT COUNT(*) FROM [Persons];");
```
### RawNonQuery And RawNonQueryAsync
You can use this method when you want to make a special non query
```
context.RawNonQuery("DELETE FROM [Persons];");
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
