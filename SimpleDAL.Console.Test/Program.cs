using SimpleDAL.Console.Test;

var context = new MyContext();

var persons = context.Persons.All();

Console.WriteLine($"Persons Count : ${persons.Count()}");
