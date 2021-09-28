using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleDAL
{
    internal class EntityCache
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _typeProperties;
        private static readonly ConcurrentDictionary<Type, IEnumerable<Attribute>> _typeAttributes;
        private static readonly ConcurrentDictionary<string, IEnumerable<Attribute>> _propertyAttributes;

        static EntityCache()
        {
            _typeProperties = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
            _typeAttributes = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();
            _propertyAttributes = new ConcurrentDictionary<string, IEnumerable<Attribute>>();
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            if (!_typeProperties.ContainsKey(type))
            {
                _typeProperties.TryAdd(type, type.GetProperties().ToList());
            }

            _typeProperties.TryGetValue(type, out var result);

            if (result == default)
            {
                result = type.GetProperties().ToList();
            }

            return result;
        }

        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            if (!_typeAttributes.ContainsKey(type))
            {
                _typeAttributes.TryAdd(type, Attribute.GetCustomAttributes(type));
            }

            _typeAttributes.TryGetValue(type, out var attributes);
            var attribute = attributes.SingleOrDefault(attr => attr.GetType().Name == typeof(T).Name);

            if (attribute == default)
            {
                return default;
            }

            return attribute as T;
        }

        public static T GetAttribute<T>(PropertyInfo propertyInfo) where T: Attribute
        {
            var propertyKey = $"{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}";

            if (!_propertyAttributes.ContainsKey(propertyKey))
            {
                _propertyAttributes.TryAdd(propertyKey, propertyInfo.GetCustomAttributes());
            }

            _propertyAttributes.TryGetValue(propertyKey, out var attributes);
            var attribute = attributes.SingleOrDefault(attr => attr.GetType().Name == typeof(T).Name);

            if (attribute == default)
            {
                return default;
            }

            return attribute as T;
        }
    }
}
