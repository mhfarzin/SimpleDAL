using System.Reflection;

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
