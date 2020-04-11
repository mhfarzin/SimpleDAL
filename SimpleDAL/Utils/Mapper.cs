using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SimpleDAL
{
    public static class Mapper<TEntity> where TEntity : class, new()
    {
        public static TEntity Map(Dictionary<string, object> rowFields)   
        {
            TEntity item = Activator.CreateInstance(typeof(TEntity)) as TEntity;
            EntityCache.GetProperties(item.GetType()).ToList().ForEach(prop =>
            {
                var propName = EntityCache.GetAttribute<ColumnAttribute>(prop)?.Name ?? prop.Name;

                if (rowFields[propName] is DBNull)
                    return;

                var safeType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                object value = default;

                if (safeType.IsEnum)
                {
                    try
                    {
                        value = Enum.Parse(safeType, rowFields[propName]?.ToString());
                    }
                    catch { }
                }
                else if (safeType.IsValueType || safeType == typeof(string))
                {
                    value = Convert.ChangeType(rowFields[propName], safeType);
                }
                else
                {
                    throw new Exception("Property Type Is Invalid");
                }

                if (value == default)
                    return;

                prop.SetValue(item, value);
            });
            return item;
        }

        public static TEntity SqlMapper(SqlDataReader reader)
        {
            var rowFields = Enumerable.Range(0, reader.FieldCount).ToDictionary(reader.GetName, reader.GetValue);
            return Map(rowFields);
        }
    }
}
