using System.Data.SqlClient;

namespace SimpleDAL.Console.Test
{
    public class MyContext : UnitOfWork
    {
        public MyContext()
            :base(Provider.SqlServer, new SqlConnection(@"Server =.\SQLEXPRESS; Database=SimpleDAL;Trusted_Connection=True;"))
        {}

        public Repository<Person> Persons { get; set; }
        public Repository<Skill> Skills { get; set; }
    }
}
