using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SimpleDAL
{
    public static class Mapper<TEntity> where TEntity : class
    {
        public static TEntity Map(DbDataReader reader)
        {
            var rowFields = Enumerable.Range(0, reader.FieldCount).ToDictionary(i => reader.GetName(i).ToLower(), reader.GetValue);
            return Map(rowFields);
        }

        private static TEntity Map(Dictionary<string, object> rowFields)
        {
            TEntity item = Activator.CreateInstance(typeof(TEntity)) as TEntity;
            EntityCache.GetProperties(item.GetType()).ToList().ForEach(prop =>
            {
                var propName = EntityCache.GetAttribute<ColumnAttribute>(prop)?.Name?.ToLower() ?? prop.Name.ToLower();
                var propIgnore = EntityCache.GetAttribute<ColumnAttribute>(prop)?.Ignore ?? false;
                var probBinary = EntityCache.GetAttribute<BinaryAttribute>(prop);

                if (propIgnore)
                    return;

                if (rowFields[propName] is DBNull)
                    return;

                var safeType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                object value = default;

                if (probBinary != default)
                {
                    value = (rowFields[propName] as byte[]).Deserialize<object>(); ;
                }
                else if (safeType.IsEnum)
                {
                    value = Enum.Parse(safeType, rowFields[propName]?.ToString());
                }
                else if (safeType.IsValueType || safeType == typeof(string))
                {
                    value = Convert.ChangeType(rowFields[propName], safeType);
                }
                else
                {
                    value = rowFields[propName];
                }

                if (value == default)
                    return;

                prop.SetValue(item, value);
            });
            return item;
        }
    }
}
