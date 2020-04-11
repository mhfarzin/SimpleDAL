using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Provider _provider;
        private readonly IDbConnection _dbConnection;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWork(string connectionString, Provider provider)
        {
            _provider = provider;

            switch (provider)
            {
                case Provider.SqlServer:
                    _dbConnection = new SqlConnection(connectionString);
                    break;
                default:
                    throw new Exception("Provider Unknow");
            };

            switch (provider)
            {
                case Provider.SqlServer: 
                    _unitOfWork = new SqlUnitOfWork(_dbConnection as SqlConnection); 
                    break;
                default:
                    throw new Exception("Provider Unknow");
            };

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

        public IEnumerable<TEntity> RawSqlQuery<TEntity>(string query, object param = default, Transaction transaction = default) where TEntity : class, new()
        {
            return _unitOfWork.RawSqlQuery<TEntity>(query, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> RawSqlQueryAsync<TEntity>(string query, object param = default, Transaction transaction = default, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _unitOfWork.RawSqlQueryAsync<TEntity>(query, param, transaction, cancellationToken);
        }

        public Transaction BeginTransaction()
        {
            return _unitOfWork.BeginTransaction();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
