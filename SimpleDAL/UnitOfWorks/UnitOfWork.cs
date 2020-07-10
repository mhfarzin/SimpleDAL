using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;

namespace SimpleDAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly Provider _provider;
        private readonly DbConnection _dbConnection;
  
        public UnitOfWork(string connectionString, Provider provider)
        {
            _provider = provider;

            _dbConnection = ConnetionFactory.GetConnection(provider, connectionString);

            var repositories = this.GetType()
               .GetProperties()
               .Where(x => IsSubclassOfRawGeneric(typeof(Repository<>), x.PropertyType))
               .ToList();

            foreach (var repository in repositories)
            {
                repository.SetValue(this, Activator.CreateInstance(repository.PropertyType, _dbConnection, _provider));
            }
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var type = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == type)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public Repository<TEntity> Set<TEntity>() where TEntity : class
        {
            var prop = this.GetType()
              .GetProperties()
              .Where(x => x.PropertyType == typeof(Repository<TEntity>))
              .FirstOrDefault();

            if (prop == default)
                throw new Exception("Repository<TEntity> Can Not Find");
                     
            return prop.GetValue(this) as Repository<TEntity>;
        }

        public IEnumerable<TEntity> RawQuery<TEntity>(string query, object param = default, Transaction transaction = default) where TEntity : class
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                return CommandExecuter.ExecuteQuery<TEntity>(_dbConnection, command);
            }
        }

        public async Task<IEnumerable<TEntity>> RawQueryAsync<TEntity>(string query, object param = default, Transaction transaction = default, CancellationToken cancellationToken = default) where TEntity : class
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                return await CommandExecuter.ExecuteQueryAsync<TEntity>(_dbConnection, command, cancellationToken);
            }
        }

        public TResult RawScalarQuery<TResult>(string query, object param = default, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                return (TResult)CommandExecuter.ExecuteScalarQuery(_dbConnection, command);
            }
        }

        public async Task<TResult> RawScalarQueryAsync<TResult>(string query, object param = default, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                return (TResult)(await CommandExecuter.ExecuteScalarQueryAsync(_dbConnection, command, cancellationToken));
            }
        }

        public void RawNonQuery(string query, object param = default, Transaction transaction = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                CommandExecuter.ExecuteNonQuery(_dbConnection, command);
            }
        }

        public async Task RawNonQueryAsync(string query, object param = default, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            using (var command = CommandDefinition.GetCommand(provider: _provider, connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                await CommandExecuter.ExecuteNonQueryAsync(_dbConnection, command, cancellationToken);
            }
        }

        public Transaction BeginTransaction()
        {
            _dbConnection.Open();
            var transaction = new Transaction(_dbConnection.BeginTransaction());
            transaction.EndTransactionEvent += () =>
            {
                if (_dbConnection.State != ConnectionState.Closed) _dbConnection.Close();
            };
            return transaction;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
