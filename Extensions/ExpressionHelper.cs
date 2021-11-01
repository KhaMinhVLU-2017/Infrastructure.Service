using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Service.Exception;

namespace Infrastructure.Service.Extension
{
    public class ExpressionHelper
    {
        private static Dictionary<string, string> _dicSorts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"asc", "OrderBy"},
            {"desc", "OrderByDescending"}
        };

        public static IQueryable<T> OrderBy<T>(IQueryable<T> source, Sort sort)
        {
            if (String.IsNullOrEmpty(sort.Key))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, typeof(T).Name);

            MemberExpression property = Expression.Property(parameter, sort.Key);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = _dicSorts[sort.Criteria];
            if (methodName == null || string.IsNullOrEmpty(methodName))
                throw new SyntaxException($"Incorrect syntax sort by key: {sort.Criteria}");

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                  new Type[] { source.ElementType, property.Type },
                                  source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}