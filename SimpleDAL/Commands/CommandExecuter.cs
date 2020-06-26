using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    public class CommandExecuter
    {
        public static IEnumerable<TEntity> ExecuteQuery<TEntity>(DbConnection dbConnection, DbCommand command) where TEntity : class, new()
        {
            var result = new List<TEntity>();
            DbDataReader reader = default;
            var wasClosed = dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) dbConnection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(Mapper<TEntity>.Map(reader));
                }
            }
            finally
            {
                if (reader != default) reader.Close();
                if (wasClosed) dbConnection.Close();
            }
            return result;
        }

        public static async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(DbConnection dbConnection, DbCommand command, CancellationToken cancellationToken) where TEntity : class, new()
        {
            var result = new List<TEntity>();
            DbDataReader reader = default;
            var wasClosed = dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await dbConnection.OpenAsync();
                reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    result.Add(Mapper<TEntity>.Map(reader));
                }
            }
            finally
            {
                if (reader != default) reader.Close();
                if (wasClosed) dbConnection.Close();
            }
            return result;
        }

        public static object ExecuteScalarQuery(DbConnection _dbConnection, DbCommand command)
        {
            var wasClosed = _dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) _dbConnection.Open();
                return command.ExecuteScalar();
            }
            finally
            {
                if (wasClosed) _dbConnection.Close();
            }
        }

        public static async Task<object> ExecuteScalarQueryAsync(DbConnection dbConnection, DbCommand command, CancellationToken cancellationToken)
        {
            var wasClosed = dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await dbConnection.OpenAsync();
                return await command.ExecuteScalarAsync(cancellationToken);
            }
            finally
            {
                if (wasClosed) dbConnection.Close();
            }
        }

        public static void ExecuteNonQuery(DbConnection dbConnection, DbCommand command)
        {
            var wasClosed = dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) dbConnection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                if (wasClosed) dbConnection.Close();
            }
        }

        public static async Task ExecuteNonQueryAsync(DbConnection dbConnection, DbCommand command, CancellationToken cancellationToken)
        {
            var wasClosed = dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) await dbConnection.OpenAsync();
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            finally
            {
                if (wasClosed) dbConnection.Close();
            }
        }

        public static List<string> GetProperticeWithAddParameters<TEntity>(Provider provider, PropertyInfo key, bool keyIsAutoId, TEntity item, DbCommand command)
            where TEntity : class, new()
        {
            var properties = new List<string>();
            EntityCache.GetProperties(item.GetType()).ToList().ForEach(prop =>
            {
                var propName = EntityCache.GetAttribute<ColumnAttribute>(prop)?.Name ?? prop.Name;
                if ((propName == key.Name && keyIsAutoId) || prop.GetValue(item) == default)
                    return;

                var propIgnore = EntityCache.GetAttribute<ColumnAttribute>(prop)?.Ignore ?? false;
                if (propIgnore)
                    return;

                properties.Add(propName);
                var value = EntityCache.GetAttribute<BinaryAttribute>(prop) == default
                    ? prop.GetValue(item)
                    : prop.GetValue(item).SerializeToByteArray();
                CommandDefinition.AddParameterWithValue(provider, command, $"@{propName}Parametr", value);
            });
            return properties;
        }
    }
}
