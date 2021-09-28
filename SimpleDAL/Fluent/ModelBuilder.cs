using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL
{
    public class ModelBuilder
    {
        public FluentConfig<TEntity> Entity<TEntity>()
        {
            return new FluentConfig<TEntity>();
        }
    }
}
