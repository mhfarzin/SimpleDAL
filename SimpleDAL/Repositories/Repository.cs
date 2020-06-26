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
    public class Repository<TEntity> where TEntity : class, new()
    {
        private readonly Provider _provider;
        private readonly DbConnection _dbConnection;
        private readonly PropertyInfo _key;
        private readonly bool _keyIsAutoId;

        public Repository(DbConnection dbConnection, Provider provider)
        {
            _provider = provider;
            _dbConnection = dbConnection;
            _key = DynamicQuery.GetIdProperty(typeof(TEntity));
            _keyIsAutoId = DynamicQuery.CheckKeyIsAutoId(_key);
        }

        public IEnumerable<TEntity> All(Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.All<TEntity>(_provider), transaction: transaction))
            {
                return CommandExecuter.ExecuteQuery<TEntity>(_dbConnection, command);
            }
        }

        public async Task<IEnumerable<TEntity>> AllAsync(Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.All<TEntity>(_provider), transaction: transaction))
            {
                return await CommandExecuter.ExecuteQueryAsync<TEntity>(_dbConnection, command, cancellationToken);
            }
        }

        public TEntity Find(object id, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.Find<TEntity>(_provider, _key), key: _key.Name, id: id, transaction: transaction))
            {
                return CommandExecuter.ExecuteQuery<TEntity>(_dbConnection, command).FirstOrDefault();
            }
        }

        public async Task<TEntity> FindAsync(object id, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.Find<TEntity>(_provider, _key), key: _key.Name, id: id, transaction: transaction))
            {
                return (await CommandExecuter.ExecuteQueryAsync<TEntity>(_dbConnection, command, cancellationToken)).FirstOrDefault();
            }
        }

        public IEnumerable<TEntity> Where(string whereCondition, object param = default, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.Where<TEntity>(_provider, whereCondition), param: param, transaction: transaction))
            {
                return CommandExecuter.ExecuteQuery<TEntity>(_dbConnection, command);
            }
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(string whereCondition, object param = default, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.Where<TEntity>(_provider, whereCondition), param: param, transaction: transaction))
            {
                return await CommandExecuter.ExecuteQueryAsync<TEntity>(_dbConnection, command, cancellationToken);
            }
        }

        public void Insert(TEntity item, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, transaction: transaction))
            {
                var properties = CommandExecuter.GetProperticeWithAddParameters<TEntity>(_provider, _key, _keyIsAutoId, item, command);
                command.CommandText = DynamicQuery.Insert(_provider, _key, properties, item);
                var key = CommandExecuter.ExecuteScalarQuery(_dbConnection, command);
                if (_keyIsAutoId)
                {
                    var value = Convert.ChangeType(key, _key.PropertyType);
                    _key.SetValue(item, value);
                }
            }
        }

        public async Task InsertAsync(TEntity item, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, transaction: transaction))
            {
                var properties = CommandExecuter.GetProperticeWithAddParameters<TEntity>(_provider, _key, _keyIsAutoId, item, command);
                command.CommandText = DynamicQuery.Insert(_provider, _key, properties, item);
                var key = await CommandExecuter.ExecuteScalarQueryAsync(_dbConnection, command, cancellationToken);
                if (_keyIsAutoId)
                {
                    var value = Convert.ChangeType(key, _key.PropertyType);
                    _key.SetValue(item, value);
                }
            }
        }

        public void Update(TEntity item, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection))
            {
                var properties = CommandExecuter.GetProperticeWithAddParameters<TEntity>(_provider, _key, _keyIsAutoId, item, command);
                command.CommandText = DynamicQuery.Update(_provider, _key, properties, item);
                CommandExecuter.ExecuteNonQuery(_dbConnection, command);
            }
        }

        public async Task UpdateAsync(TEntity item, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, transaction: transaction))
            {
                var properties = CommandExecuter.GetProperticeWithAddParameters<TEntity>(_provider, _key, _keyIsAutoId, item, command);
                command.CommandText = DynamicQuery.Update(_provider, _key, properties, item);
                await CommandExecuter.ExecuteNonQueryAsync(_dbConnection, command, cancellationToken);
            }
        }

        public void Delete(object id, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.Delete<TEntity>(_provider, _key), key: _key.Name, id: id, transaction: transaction))
            {
                CommandExecuter.ExecuteNonQuery(_dbConnection, command);
            }
        }

        public async Task DeleteAsync(object id, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: DynamicQuery.Delete<TEntity>(_provider, _key), _key.Name, id: id, transaction: transaction))
            {
                await CommandExecuter.ExecuteNonQueryAsync(_dbConnection, command, cancellationToken);
            }
        }
    }
}
