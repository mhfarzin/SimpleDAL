using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SimpleDAL
{
    public class Transaction : IDisposable
    {
        private readonly DbTransaction _dbTransaction;

        internal event Action EndTransactionEvent;

        public Transaction(DbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        internal DbTransaction Get()
        {
            return _dbTransaction;
        }

        public void Commit()
        {
            _dbTransaction.Commit();
            if (EndTransactionEvent != default) EndTransactionEvent();
        }

        public void RollBack()
        {
            _dbTransaction.Rollback();
            if (EndTransactionEvent != default) EndTransactionEvent();
        }

        public void Dispose()
        {
            if (EndTransactionEvent != default) EndTransactionEvent();
            GC.SuppressFinalize(this);
        }
    }
}
