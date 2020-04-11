using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    internal interface IRepository<TEntity>
    {
        IEnumerable<TEntity> All(Transaction transaction);
        Task<IEnumerable<TEntity>> AllAsync(Transaction transaction, CancellationToken cancellationToken);
        TEntity Find(object id, Transaction transaction);
        Task<TEntity> FindAsync(object id, Transaction transaction, CancellationToken cancellationToken);
        IEnumerable<TEntity> Where(string whereCondition, object param, Transaction transaction);
        Task<IEnumerable<TEntity>> WhereAsync(string whereCondition, object param, Transaction transaction, CancellationToken cancellationToken);
        void Insert(TEntity item, Transaction transaction);
        Task InsertAsync(TEntity item, Transaction transaction, CancellationToken cancellationToken);
        void Update(TEntity item, Transaction transaction);
        Task UpdateAsync(TEntity item, Transaction transaction, CancellationToken cancellationToken);
        void Delete(object id, Transaction transaction);
        Task DeleteAsync(object id, Transaction transaction, CancellationToken cancellationToken);
    }
}
