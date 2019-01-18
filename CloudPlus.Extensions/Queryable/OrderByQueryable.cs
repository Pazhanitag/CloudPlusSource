using System;
using System.Linq;
using System.Linq.Expressions;

namespace CloudPlus.Extensions.Queryable
{
    public static class OrderByQueryable
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> collection, string orderByColumn, string orderType)
        {
            if (string.IsNullOrWhiteSpace(orderByColumn))
                throw new ArgumentNullException(nameof(orderByColumn));

            if (string.IsNullOrWhiteSpace(orderType))
                throw new ArgumentNullException(nameof(orderType));

            if (orderType != "asc" && orderType != "desc")
                throw new NotSupportedException("Invalid order type");

            var collectionType = typeof(T);
            var orderByMethodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(collectionType);

            var propertyOrFieldExpression =
                Expression.PropertyOrField(parameterExpression, orderByColumn);

            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(System.Linq.Queryable), orderByMethodName,
                new[] { collectionType, propertyOrFieldExpression.Type }, collection.Expression, selector);

            return collection.Provider.CreateQuery<T>(orderByExpression);
        }
    }
}