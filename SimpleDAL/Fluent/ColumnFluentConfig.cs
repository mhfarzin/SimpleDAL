using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleDAL
{
    public class ColumnFluentConfig<TEntity>
    {
        private readonly PropertyInfo _property;

        public ColumnFluentConfig(PropertyInfo property)
        {
            _property = property;

            if (FluentCache.GetAttribute<ColumnAttribute>(property) == default)
            {
                FluentCache.SetAttribute(property, new ColumnAttribute());
            }
        }

        public ColumnFluentConfig<TEntity> Name(string name)
        {
            var attribute = FluentCache.GetAttribute<ColumnAttribute>(_property);
            attribute.Name = name;
            FluentCache.SetAttribute(_property, attribute);
            return this;
        }

        public ColumnFluentConfig<TEntity> Ignore(bool ignore = true)
        {
            var attribute = FluentCache.GetAttribute<ColumnAttribute>(_property);
            attribute.Ignore = ignore;
            FluentCache.SetAttribute(_property, attribute);
            return this;
        }
    }
}
