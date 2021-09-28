using System.Reflection;

namespace SimpleDAL
{
    public class KeyFluentConfig<TEntity>
    {
        private readonly PropertyInfo _property;

        public KeyFluentConfig(PropertyInfo property)
        {
            _property = property;

            if (FluentCache.GetAttribute<KeyAttribute>(property) == default)
            {
                FluentCache.SetAttribute(property, new KeyAttribute());
            }
        }

        public KeyFluentConfig<TEntity> AutoIdentity(bool autoIdentity = true)
        {
            var attribute = FluentCache.GetAttribute<KeyAttribute>(_property);
            attribute.AutoIdentity = autoIdentity;
            FluentCache.SetAttribute(_property, attribute);
            return this;
        }
    }
}
