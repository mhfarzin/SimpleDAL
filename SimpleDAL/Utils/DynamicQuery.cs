using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleDAL
{
    internal class DynamicQuery
    {
        public static PropertyInfo GetIdProperty(Type type)
        {
            PropertyInfo result;

            var properties = EntityCache.GetProperties(type).Where(prop => EntityCache.GetAttribute<KeyAttribute>(prop) != default).ToList();

            result = properties.Any()
                ? properties.First()
                : EntityCache.GetProperties(type).SingleOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

            if (result == default)
            {
                throw new Exception("entity must have key attribute field or Id name field.");
            }

            return result;
        }

        public static bool CheckKeyIsAutoId(PropertyInfo prop)
        {
            return EntityCache.GetAttribute<KeyAttribute>(prop).AutoIdentity;
        }

        public static string All<TEntity>() =>
            $"SELECT * FROM [{GetTableName(typeof(TEntity))}]";

        public static string Where<TEntity>(string whereCondition) =>
            $"SELECT * FROM [{GetTableName(typeof(TEntity))}] WHERE {whereCondition}";

        public static string Find<TEntity>(PropertyInfo key) =>
            $"SELECT * FROM [{GetTableName(typeof(TEntity))}] WHERE {key.Name}=@{key.Name}Parametr";

        public static string Insert<TEntity>(PropertyInfo key, List<string> properties, TEntity item) =>
               $"INSERT INTO [{GetTableName(typeof(TEntity))}] ({string.Join(",", properties.Select(x => $"[{x}]"))}) OUTPUT Inserted.{key.Name} VALUES ({string.Join(",", properties.Select(x => $"@{x}Parametr"))})";

        public static string Update<TEntity>(PropertyInfo key, List<string> properties, TEntity item) =>
                $"UPDATE [{GetTableName(typeof(TEntity))}] Set {string.Join(",", properties.Select(x => $"[{x}]=@{x}Parametr"))} WHERE {key.Name}={key.GetValue(item)}";

        public static string Delete<TEntity>(PropertyInfo key) =>
            $"DELETE FROM [{GetTableName(typeof(TEntity))}] WHERE {key.Name}=@{key.Name}Parametr";

        private static string GetTableName(Type type)
        {
            return EntityCache.GetAttribute<TableAttribute>(type)?.Name ?? type.Name;
        }
    }
}
