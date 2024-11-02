using EnerSync.Data;
using EnerSync.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnerSync.Services
{
    public class FilterService
    {
        private readonly EnerSyncContext _context;

        public FilterService()
        {
            _context = null!;
        }

        public FilterService(EnerSyncContext context)
        {
            _context = context;
        }

        public IAsyncEnumerable<T> GetFilteredDataAsync<T>(List<FilterCriteria> criteria) where T : class
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            Expression<Func<T, bool>> combinedExpression = null!;

            foreach (var criterion in criteria)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = typeof(T).GetProperty(criterion.FieldName) ?? throw new ArgumentException($"Property '{criterion.FieldName}' does not exist on type '{typeof(T).Name}'");
                var propertyAccess = Expression.Property(parameter, property);

                // Get the underlying (non-nullable) type for compatibility
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                // Convert the criterion's value to the exact property type for comparison
                var rawValue = criterion.GetTypedValue();

                // Handle nullable properties by ensuring `typedValue` matches property type
                Expression typedValue = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Expression.Convert(Expression.Constant(rawValue), property.PropertyType)
                    : Expression.Constant(rawValue, propertyAccess.Type);

                // Handle DateOnly-specific comparison expressions
                Expression comparison = criterion.Operator switch
                {
                    "Equals" => Expression.Equal(propertyAccess, typedValue),
                    "NotEquals" => Expression.NotEqual(propertyAccess, typedValue),
                    "GreaterThan" => Expression.GreaterThan(propertyAccess, typedValue),
                    "GreaterThanOrEqual" => Expression.GreaterThanOrEqual(propertyAccess, typedValue),
                    "LessThan" => Expression.LessThan(propertyAccess, typedValue),
                    "LessThanOrEqual" => Expression.LessThanOrEqual(propertyAccess, typedValue),
                    "IsNull" => Expression.Equal(propertyAccess, Expression.Constant(null, propertyAccess.Type)),
                    "IsNotNull" => Expression.NotEqual(propertyAccess, Expression.Constant(null, propertyAccess.Type)),

                    // String-specific operators
                    "Contains" when property.PropertyType == typeof(string) =>
                        Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", [typeof(string)])!, typedValue),

                    "DoesNotContain" when property.PropertyType == typeof(string) =>
                        Expression.Not(Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", [typeof(string)])!, typedValue)),

                    "StartsWith" when property.PropertyType == typeof(string) =>
                        Expression.Call(propertyAccess, typeof(string).GetMethod("StartsWith", [typeof(string)])!, typedValue),

                    "EndsWith" when property.PropertyType == typeof(string) =>
                        Expression.Call(propertyAccess, typeof(string).GetMethod("EndsWith", [typeof(string)])!, typedValue),

                    // Check if the string is empty
                    "IsEmpty" when property.PropertyType == typeof(string) =>
                        Expression.Equal(propertyAccess, Expression.Constant(string.Empty)),

                    // Check if the string is not empty
                    "IsNotEmpty" when property.PropertyType == typeof(string) =>
                        Expression.NotEqual(propertyAccess, Expression.Constant(string.Empty)),

                    _ => throw new NotSupportedException($"Operator '{criterion.Operator}' is not supported for {property.PropertyType.Name}")
                };

                // Apply NOT operator if specified in LogicalOperator
                if (criterion.LogicalOperator == "NOT")
                {
                    comparison = Expression.Not(comparison);
                }

                if (combinedExpression == null)
                {
                    combinedExpression = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                }
                else
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                    combinedExpression = criterion.LogicalOperator switch
                    {
                        "AND" => Expression.Lambda<Func<T, bool>>(Expression.AndAlso(combinedExpression.Body, lambda.Body), parameter),
                        "OR" => Expression.Lambda<Func<T, bool>>(Expression.OrElse(combinedExpression.Body, lambda.Body), parameter),
                        _ => combinedExpression // Default to combinedExpression if LogicalOperator is not recognized
                    };
                }
            }

            // Apply the combined expression to the query
            if (combinedExpression != null)
            {
                query = query.Where(combinedExpression);
            }

            return query.AsAsyncEnumerable();
        }
    }
}
