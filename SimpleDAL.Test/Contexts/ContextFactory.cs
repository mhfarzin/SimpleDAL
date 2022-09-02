using SimpleDAL.Test.Contexts;

namespace SimpleDAL.Test
{
    public class ContextFactory
    {
        public const string SqlServerConnectionString = @"Server =.\SQLEXPRESS; Database=SimpleDAL;Trusted_Connection=True;";
        public const string MySqlConnectionString = @"server=localhost;user=root;database=simpledal;port=3306;password=Aa123456";
        public const string SQLiteConnectionString = @"URI=file:E:\SQLite\SimpleDal.db";

        public static TestSqlServerContext GetSqlServerContext()
        {
            return new TestSqlServerContext(SqlServerConnectionString);
        }
        
        public static TestMySqlContext GetMySqlContext()
        {
            return new TestMySqlContext(MySqlConnectionString);
        }

        public static TestSQLiteContext GetSqlLiteContext()
        {
            return new TestSQLiteContext(SQLiteConnectionString);
        }

        public static TestFluentContext GetTestFluentContext()
        {
            return new TestFluentContext(SQLiteConnectionString);
        }

    }
}
