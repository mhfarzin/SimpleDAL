using System.Data;
using System.Data.Common;
using System.Linq;

namespace SimpleDAL
{
    internal class CommandDefinition
    {
        public static DbCommand GetCommand(DbConnection connection, string commandText = default, string key = default, object id = default, object param = default, Transaction transaction = default)
        {
            var command = connection.CreateCommand();

            if (!string.IsNullOrEmpty(commandText))
            {
                command.CommandText = commandText;
            }

            if (key != default && id != default)
            {
                AddParameterWithValue(command, $"@{key}Parametr", id);
            }

            if (param != default)
            {
                param.GetType().GetProperties().ToList().ForEach(prop =>
                {
                    AddParameterWithValue(command, $"@{prop.Name}", prop.GetValue(param));
                });
            }

            if (transaction != default)
            {
                command.Transaction = transaction.Get();
            }

            return command;
        }

        public static void AddParameterWithValue(DbCommand dbCommand, string name, object value)
        {
            IDbDataParameter dataParametr = dbCommand.CreateParameter();
            dataParametr.ParameterName = name;
            dataParametr.Value = value;
            dbCommand.Parameters.Add(dataParametr);
        }
    }
}
