using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleDAL
{
    public class BinaryFluentConfig<TEntity>
    {
        private readonly PropertyInfo _property;

        public BinaryFluentConfig(PropertyInfo property)
        {
            _property = property;

            if (FluentCache.GetAttribute<BinaryAttribute>(property) == default)
            {
                FluentCache.SetAttribute(property, new BinaryAttribute());
            }
        }
    }
}
