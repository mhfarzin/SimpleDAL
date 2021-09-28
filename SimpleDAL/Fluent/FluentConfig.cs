using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SimpleDAL
{
    public class FluentConfig<TEntity>
    {
        public TableFluentConfig<TEntity> Table()
        {
            return new TableFluentConfig<TEntity>();
        }

        public KeyFluentConfig<TEntity> Key(Expression<Func<TEntity, object>> propertyLambda)
        {
            var property = GetPropertyInfoFromExpersion(propertyLambda);
            return new KeyFluentConfig<TEntity>(property);
        }

        public ColumnFluentConfig<TEntity> Column(Expression<Func<TEntity, object>> propertyLambda)
        {
            var property = GetPropertyInfoFromExpersion(propertyLambda);
            return new ColumnFluentConfig<TEntity>(property);
        }

        public BinaryFluentConfig<TEntity> Binary(Expression<Func<TEntity, object>> propertyLambda)
        {
            var property = GetPropertyInfoFromExpersion(propertyLambda);
            return new BinaryFluentConfig<TEntity>(property);
        }

        private PropertyInfo GetPropertyInfoFromExpersion(Expression<Func<TEntity, object>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression = (lambda.Body is UnaryExpression unaryExpression)
                ? (MemberExpression)unaryExpression.Operand
                : (MemberExpression)lambda.Body;

            return (PropertyInfo)memberExpression.Member;
        }
    }
}
