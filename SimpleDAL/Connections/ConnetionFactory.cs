using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Data.SqlClient;

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
                default:
                    throw new Exception("Provider Unknow");
            };
        }
    }
}
