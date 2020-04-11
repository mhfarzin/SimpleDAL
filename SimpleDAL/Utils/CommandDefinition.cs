using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SimpleDAL
{
    public class CommandDefinition
    {
        public static SqlCommand GetCommand(SqlConnection connection, string commandText = default, string key = default, object id = default, object param = default, Transaction transaction = default)
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
    }
}
