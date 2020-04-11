using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly IRepository<TEntity> _repository;

        public Repository(IDbConnection dbConnection, Provider provider)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    _repository = new SqlRepository<TEntity>(dbConnection as SqlConnection);
                    break;
                default:
                    throw new Exception("Provider Unknow");
            };
        }

        public IEnumerable<TEntity> All(Transaction transaction = default)
        {
            return _repository.All(transaction);
        }

        public async Task<IEnumerable<TEntity>> AllAsync(Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            return await _repository.AllAsync(transaction, cancellationToken);
        }

        public TEntity Find(object id, Transaction transaction = default)
        {
            return _repository.Find(id, transaction);
        }

        public async Task<TEntity> FindAsync(object id, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            return await _repository.FindAsync(id, transaction, cancellationToken);
        }

        public IEnumerable<TEntity> Where(string whereCondition, object param = default, Transaction transaction = default)
        {
            return _repository.Where(whereCondition, param, transaction);
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(string whereCondition, object param = default, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            return await _repository.WhereAsync(whereCondition, param, transaction, cancellationToken);
        }

        public void Insert(TEntity item, Transaction transaction = default)
        {
            _repository.Insert(item, transaction);
        }

        public async Task InsertAsync(TEntity item, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            await _repository.InsertAsync(item, transaction, cancellationToken);
        }

        public void Update(TEntity item, Transaction transaction = default)
        {
            _repository.Update(item, transaction);
        }

        public async Task UpdateAsync(TEntity item, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            await _repository.UpdateAsync(item, transaction, cancellationToken);
        }

        public void Delete(object id, Transaction transaction = default)
        {
            _repository.Delete(id, transaction);
        }

        public async Task DeleteAsync(object id, Transaction transaction = default, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteAsync(id, transaction, cancellationToken);
        }
    }
}
