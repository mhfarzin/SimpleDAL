using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleDAL
{
    internal interface IUnitOfWork
    {
        IEnumerable<TEntity> RawSqlQuery<TEntity>(string query, object param, Transaction transaction) where TEntity : class, new();
        Task<IEnumerable<TEntity>> RawSqlQueryAsync<TEntity>(string query, object param, Transaction transaction, CancellationToken cancellationToken) where TEntity : class, new();
        Transaction BeginTransaction();
    }
}
