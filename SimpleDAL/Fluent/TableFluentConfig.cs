using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDAL
{
    public class TableFluentConfig<TEntity>
    {
        public TableFluentConfig()
        {
            if (FluentCache.GetAttribute<TableAttribute>(typeof(TEntity)) == default)
            {
                FluentCache.SetAttribute(typeof(TEntity), new TableAttribute());
            }
        }

        public TableFluentConfig<TEntity> Name(string name)
        {
            var attribute = FluentCache.GetAttribute<TableAttribute>(typeof(TEntity));
            attribute.Name = name;
            FluentCache.SetAttribute(typeof(TEntity), attribute);
            return this;
        }
    }
}
