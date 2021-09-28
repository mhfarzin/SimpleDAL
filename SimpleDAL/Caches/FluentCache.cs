using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleDAL
{
    internal class FluentCache
    {
        private static readonly ConcurrentDictionary<Type, IList<Attribute>> _typeAttributes;
        private static readonly ConcurrentDictionary<string, IList<Attribute>> _propertyAttributes;

        static FluentCache()
        {
            _typeAttributes = new ConcurrentDictionary<Type, IList<Attribute>>();
            _propertyAttributes = new ConcurrentDictionary<string, IList<Attribute>>();
        }

        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            _typeAttributes.TryGetValue(type, out var attributes);
            if (attributes == default)
                return default;

            var attribute = attributes.SingleOrDefault(attr => attr.GetType().Name == typeof(T).Name);
            if (attribute == default)
                return default;

            return attribute as T;
        }

        public static void SetAttribute<T>(Type type, T attribute)
            where T : Attribute
        {
            _typeAttributes.TryGetValue(type, out var attributes);
            if (attributes == default)
            {
                attributes = new List<Attribute>();
            }

            var oldAttribute = attributes.OfType<T>().SingleOrDefault();
            if (oldAttribute != default)
            {
                attributes.Remove(oldAttribute);
            }

            attributes.Add(attribute);
            _typeAttributes[type] = attributes;
        }

        public static T GetAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
        {
            var propertyKey = $"{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}";

            _propertyAttributes.TryGetValue(propertyKey, out var attributes);
            if (attributes == default)
                return default;

            var attribute = attributes.SingleOrDefault(attr => attr.GetType().Name == typeof(T).Name);
            if (attribute == default)
                return default;

            return attribute as T;
        }

        public static void SetAttribute<T>(PropertyInfo propertyInfo, T attribute)
            where T : Attribute
        {
            var propertyKey = $"{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}";
            _propertyAttributes.TryGetValue(propertyKey, out var attributes);
            if (attributes == default)
            {
                attributes = new List<Attribute>();
            }

            var oldAttribute = attributes.OfType<T>().SingleOrDefault();
            if (oldAttribute != default)
            {
                attributes.Remove(oldAttribute);
            }

            attributes.Add(attribute);
            _propertyAttributes[propertyKey] = attributes;
        }
    }
}
