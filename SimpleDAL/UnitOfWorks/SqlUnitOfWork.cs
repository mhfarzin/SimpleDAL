using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    internal class SqlUnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _dbConnection;

        public SqlUnitOfWork(SqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<TEntity> RawSqlQuery<TEntity>(string query, object param, Transaction transaction = default) where TEntity : class, new()
        {
            var result = new List<TEntity>();
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                SqlDataReader reader = null;
                try
                {
                    _dbConnection.Open();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(Mapper<TEntity>.SqlMapper(reader));
                    }
                }
                finally
                {
                    if (reader != default)
                    {
                        reader.Close();
                    }
                    if (_dbConnection.State == ConnectionState.Open)
                    {
                        _dbConnection.Close();
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<TEntity>> RawSqlQueryAsync<TEntity>(string query, object param, Transaction transaction = default, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var result = new List<TEntity>();
            using (var command = CommandDefinition.GetCommand(connection: _dbConnection, commandText: query, param: param, transaction: transaction))
            {
                SqlDataReader reader = null;
                try
                {
                    _dbConnection.Open();
                    reader = await command.ExecuteReaderAsync(cancellationToken);
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        result.Add(Mapper<TEntity>.SqlMapper(reader));
                    }
                }
                finally
                {
                    if (reader != default)
                    {
                        reader.Close();
                    }
                    if (_dbConnection.State == ConnectionState.Open)
                    {
                        _dbConnection.Close();
                    }
                }
            }
            return result;
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
    }
}