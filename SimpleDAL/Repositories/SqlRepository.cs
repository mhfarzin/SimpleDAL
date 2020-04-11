using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    internal class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly SqlConnection _dbConnection;
        private readonly PropertyInfo _key;
        private readonly bool _keyIsAutoId;

        public SqlRepository(SqlConnection connection)
        {
            _dbConnection = connection;
            _key = DynamicQuery.GetIdProperty(typeof(TEntity));
            _keyIsAutoId = DynamicQuery.CheckKeyIsAutoId(_key);
        }

        public IEnumerable<TEntity> All(Transaction transaction)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.All<TEntity>(), transaction: transaction))
            {
                return ExecuteRead(command);
            }
        }

        public async Task<IEnumerable<TEntity>> AllAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.All<TEntity>(), transaction: transaction))
            {
                return await ExecuteReadAsync(command, cancellationToken);
            }
        }

        public TEntity Find(object id, Transaction transaction)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.Find<TEntity>(_key), key: _key.Name, id: id, transaction: transaction))
            {
                return ExecuteRead(command).FirstOrDefault();
            }
        }

        public async Task<TEntity> FindAsync(object id, Transaction transaction, CancellationToken cancellationToken)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.Find<TEntity>(_key), key: _key.Name, id: id, transaction: transaction))
            {
                return (await ExecuteReadAsync(command, cancellationToken)).FirstOrDefault();
            }
        }

        public IEnumerable<TEntity> Where(string whereCondition, object param, Transaction transaction)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.Where<TEntity>(whereCondition), param: param, transaction: transaction))
            {
                return ExecuteRead(command);
            }
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(string whereCondition, object param, Transaction transaction, CancellationToken cancellationToken)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.Where<TEntity>(whereCondition), param: param, transaction: transaction))
            {
                return await ExecuteReadAsync(command, cancellationToken);
            }
        }

        public void Insert(TEntity item, Transaction transaction)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, transaction: transaction))
            {
                var properties = GetProperticeWithAddParameters(item, command);
                command.CommandText = DynamicQuery.Insert(_key, properties, item);
                var key = ExecuteScalar(command);
                _key.SetValue(item, key);
            }
        }

        public async Task InsertAsync(TEntity item, Transaction transaction, CancellationToken cancellationToken)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, transaction: transaction))
            {
                var properties = GetProperticeWithAddParameters(item, command);
                command.CommandText = DynamicQuery.Insert(_key, properties, item);
                var key = await ExecuteScalarAsync(command, cancellationToken);
                _key.SetValue(item, key);
            }
        }

        public void Update(TEntity item, Transaction transaction)
        {
            using (var command = CommandDefinition.GetCommand(_dbConnection))
            {
                var properties = GetProperticeWithAddParameters(item, command);
                command.CommandText = DynamicQuery.Update(_key, properties, item);
                ExecuteCommand(command);
            }
        }

        public async Task UpdateAsync(TEntity item, Transaction transaction, CancellationToken cancellationToken)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, transaction: transaction))
            {
                var properties = GetProperticeWithAddParameters(item, command);
                command.CommandText = DynamicQuery.Update(_key, properties, item);
                await ExecuteCommandAsync(command, cancellationToken);
            }
        }

        public void Delete(object id, Transaction transaction)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.Delete<TEntity>(_key), key: _key.Name, id: id, transaction: transaction))
            {
                ExecuteCommand(command);
            }
        }

        public async Task DeleteAsync(object id, Transaction transaction, CancellationToken cancellationToken)
        {
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: DynamicQuery.Delete<TEntity>(_key), _key.Name, id: id, transaction: transaction))
            {
                await ExecuteCommandAsync(command, cancellationToken);
            }
        }

        private IEnumerable<TEntity> ExecuteRead(SqlCommand command)
        {
            var result = new List<TEntity>();
            SqlDataReader reader = default;
            var wasClosed = _dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) _dbConnection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(Mapper<TEntity>.SqlMapper(reader));
                }
            }
            finally
            {
                if (reader != default) reader.Close();
                if (wasClosed) _dbConnection.Close();
            }
            return result;
        }

        private async Task<IEnumerable<TEntity>> ExecuteReadAsync(SqlCommand command, CancellationToken cancellationToken)
        {
            var result = new List<TEntity>();
            SqlDataReader reader = default;
            var wasClosed = _dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) _dbConnection.Open();
                reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    result.Add(Mapper<TEntity>.SqlMapper(reader));
                }
            }
            finally
            {
                if (reader != default) reader.Close();
                if (wasClosed) _dbConnection.Close();
            }
            return result;
        }

        private object ExecuteScalar(SqlCommand command)
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

        private async Task<object> ExecuteScalarAsync(SqlCommand command, CancellationToken cancellationToken)
        {
            var wasClosed = _dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) _dbConnection.Open();
                return await command.ExecuteScalarAsync(cancellationToken);
            }
            finally
            {
                if (wasClosed) _dbConnection.Close();
            }
        }

        private void ExecuteCommand(SqlCommand command)
        {
            var wasClosed = _dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) _dbConnection.Open();
                command.ExecuteNonQuery();
            }
            finally
            {
                if (wasClosed) _dbConnection.Close();
            }
        }

        private async Task ExecuteCommandAsync(SqlCommand command, CancellationToken cancellationToken)
        {
            var wasClosed = _dbConnection.State == ConnectionState.Closed;
            try
            {
                if (wasClosed) _dbConnection.Open();
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            finally
            {
                if (wasClosed) _dbConnection.Close();
            }
        }

        private List<string> GetProperticeWithAddParameters(TEntity item, SqlCommand command)
        {
            var properties = new List<string>();
            EntityCache.GetProperties(item.GetType()).ToList().ForEach(prop =>
            {
                var propName = EntityCache.GetAttribute<ColumnAttribute>(prop)?.Name ?? prop.Name;

                if ((propName == _key.Name && _keyIsAutoId) || prop.GetValue(item) == null)
                    return;

                properties.Add(propName);
                command.Parameters.AddWithValue($"@{propName}Parametr", prop.GetValue(item));
            });
            return properties;
        }
    }
}
