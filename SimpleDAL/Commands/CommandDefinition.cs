using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace SimpleDAL
{
    internal class CommandDefinition
    {

        public static DbCommand GetCommand(Provider provider, IDbConnection connection, string commandText = default, string key = default, object id = default, object param = default, Transaction transaction = default)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return GetCommand(connection as SqlConnection, commandText, key, id, param, transaction);
                case Provider.MySql:
                    return GetCommand(connection as MySqlConnection, commandText, key, id, param, transaction);
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        public static void AddParameterWithValue(Provider provider, DbCommand dbCommand, string name, object value)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    (dbCommand as SqlCommand).Parameters.AddWithValue(name, value);
                    break;
                case Provider.MySql:
                    (dbCommand as MySqlCommand).Parameters.AddWithValue(name, value);
                    break;
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        private static SqlCommand GetCommand(SqlConnection connection, string commandText = default, string key = default, object id = default, object param = default, Transaction transaction = default)
        {
            SqlCommand command = new SqlCommand()
            {
                Connection = connection
            };

            if (!string.IsNullOrEmpty(commandText))
            {
                command.CommandText = commandText;
            }

            if (key != default && id != default)
            {
                command.Parameters.AddWithValue($"@{key}Parametr", id);
            }

            if (param != default)
            {
                param.GetType().GetProperties().ToList().ForEach(prop =>
                {
                    command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(param));
                });
            }

            if (transaction != default)
            {
                command.Transaction = transaction.Get() as SqlTransaction;
            }

            return command;
        }

        private static MySqlCommand GetCommand(MySqlConnection connection, string commandText = default, string key = default, object id = default, object param = default, Transaction transaction = default)
        {
            MySqlCommand command = new MySqlCommand()
            {
                Connection = connection
            };

            if (!string.IsNullOrEmpty(commandText))
            {
                command.CommandText = commandText;
            }

            if (key != default && id != default)
            {
                command.Parameters.AddWithValue($"@{key}Parametr", id);
            }

            if (param != default)
            {
                param.GetType().GetProperties().ToList().ForEach(prop =>
                {
                    command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(param));
                });
            }

            if (transaction != default)
            {
                command.Transaction = transaction.Get() as MySqlTransaction;
            }

            return command;
        }
    }
}
