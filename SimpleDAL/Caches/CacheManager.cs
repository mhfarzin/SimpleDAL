using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleDAL
{
    internal class CacheManager
    {
        public static IEnumerable<PropertyInfo> GetProperties(Type type)
            => EntityCache.GetProperties(type);

        public static T GetAttribute<T>(Type type) where T : Attribute
            => EntityCache.GetAttribute<T>(type) ?? FluentCache.GetAttribute<T>(type);

        public static T GetAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
            => EntityCache.GetAttribute<T>(propertyInfo) ?? FluentCache.GetAttribute<T>(propertyInfo);
    }
}
