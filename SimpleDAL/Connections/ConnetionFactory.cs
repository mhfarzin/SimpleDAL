using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace SimpleDAL
{
    internal class ConnetionFactory
    {
        public static DbConnection GetConnection(Provider provider, string connectionString)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return new SqlConnection(connectionString);
                case Provider.MySql:
                    return new MySqlConnection(connectionString);
                case Provider.SQLite:
                    return new SQLiteConnection(connectionString);
                default:
                    throw new Exception("Provider Unknow");
            };
        }
    }
}
