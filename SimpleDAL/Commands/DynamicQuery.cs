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

            var properties = CacheManager.GetProperties(type).Where(prop => CacheManager.GetAttribute<KeyAttribute>(prop) != default).ToList();

            result = properties.Any()
                ? properties.First()
                : CacheManager.GetProperties(type).SingleOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

            if (result == default)
            {
                throw new Exception("entity must have key attribute field or Id name field.");
            }

            return result;
        }

        public static bool CheckKeyIsAutoId(PropertyInfo prop)
        {
            return CacheManager.GetAttribute<KeyAttribute>(prop)?.AutoIdentity ?? true;
        }

        public static string All<TEntity>(Provider provider)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return $"SELECT * FROM [{GetTableName(typeof(TEntity))}];";
                case Provider.MySql:
                    return $"SELECT * FROM `{GetTableName(typeof(TEntity)).ToLower()}`;";
                case Provider.SQLite:
                    return $"SELECT * FROM [{GetTableName(typeof(TEntity))}];";
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        public static string Where<TEntity>(Provider provider, string whereCondition)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return $"SELECT * FROM [{GetTableName(typeof(TEntity))}] WHERE {whereCondition};";
                case Provider.MySql:
                    return $"SELECT * FROM `{GetTableName(typeof(TEntity)).ToLower()}` WHERE {whereCondition};";
                case Provider.SQLite:
                    return $"SELECT * FROM [{GetTableName(typeof(TEntity))}] WHERE {whereCondition};";
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        public static string Find<TEntity>(Provider provider, PropertyInfo key)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return $"SELECT * FROM [{GetTableName(typeof(TEntity))}] WHERE [{key.Name}]=@{key.Name}Parametr;";
                case Provider.MySql:
                    return $"SELECT * FROM `{GetTableName(typeof(TEntity)).ToLower()}` WHERE `{key.Name.ToLower()}`=@{key.Name}Parametr;";
                case Provider.SQLite:
                    return $"SELECT * FROM [{GetTableName(typeof(TEntity))}] WHERE [{key.Name}]=@{key.Name}Parametr;";
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        public static string Insert<TEntity>(Provider provider, PropertyInfo key, List<string> properties, TEntity item)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return $"INSERT INTO [{GetTableName(typeof(TEntity))}] ({string.Join(",", properties.Select(x => $"[{x}]"))}) OUTPUT Inserted.{key.Name} VALUES ({string.Join(",", properties.Select(x => $"@{x}Parametr"))});";
                case Provider.MySql:
                    return $"INSERT INTO `{GetTableName(typeof(TEntity)).ToLower()}` ({string.Join(",", properties.Select(x => $"`{x.ToLower()}`"))}) VALUES ({string.Join(",", properties.Select(x => $"@{x}Parametr"))});SELECT LAST_INSERT_ID();";
                case Provider.SQLite:
                    return $"INSERT INTO [{GetTableName(typeof(TEntity))}] ({string.Join(",", properties.Select(x => $"[{x}]"))}) VALUES ({string.Join(",", properties.Select(x => $"@{x}Parametr"))});SELECT seq from sqlite_sequence where name ='{GetTableName(typeof(TEntity))}';";
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        public static string Update<TEntity>(Provider provider, PropertyInfo key, List<string> properties, TEntity item)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return $"UPDATE [{GetTableName(typeof(TEntity))}] Set {string.Join(",", properties.Select(x => $"[{x}]=@{x}Parametr"))} WHERE [{key.Name}]={key.GetValue(item)};";
                case Provider.MySql:
                    return $"UPDATE `{GetTableName(typeof(TEntity)).ToLower()}` Set {string.Join(",", properties.Select(x => $"`{x.ToLower()}`=@{x}Parametr"))} WHERE `{key.Name.ToLower()}`={key.GetValue(item)};";
                case Provider.SQLite:
                    return $"UPDATE [{GetTableName(typeof(TEntity))}] Set {string.Join(",", properties.Select(x => $"[{x}]=@{x}Parametr"))} WHERE [{key.Name}]={key.GetValue(item)};";
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        public static string Delete<TEntity>(Provider provider, PropertyInfo key)
        {
            switch (provider)
            {
                case Provider.SqlServer:
                    return $"DELETE FROM [{GetTableName(typeof(TEntity))}] WHERE [{key.Name}]=@{key.Name}Parametr;";
                case Provider.MySql:
                    return $"DELETE FROM `{GetTableName(typeof(TEntity)).ToLower()}` WHERE `{key.Name.ToLower()}`=@{key.Name}Parametr;";
                case Provider.SQLite:
                    return $"DELETE FROM `{GetTableName(typeof(TEntity)).ToLower()}` WHERE `{key.Name.ToLower()}`=@{key.Name}Parametr;";
                default:
                    throw new Exception("Provider Unknow");
            }
        }

        private static string GetTableName(Type type)
        {
            return CacheManager.GetAttribute<TableAttribute>(type)?.Name ?? type.Name;
        }
    }
}
