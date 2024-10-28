using EnerSync.Data;
using EnerSync.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnerSync.Services
{
    public class FilterService
    {
        private readonly EnerSyncContext _context;

        public FilterService(EnerSyncContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetFilteredDataAsync<T>(List<FilterCriteria> filterCriteria) where T : class
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var criterion in filterCriteria)
            {
                query = query.Where(BuildFilterExpression<T>(criterion));
            }

            // Return results after applying filters on the server
            return await query.Take(1000000).ToListAsync();
        }

        private Expression<Func<T, bool>> BuildFilterExpression<T>(FilterCriteria criterion) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, criterion.FieldName!);

            // Ensure compatibility with server-side execution by using safe conversions
            object convertedValue = Convert.ChangeType(criterion.Value!, property.Type!);
            var constant = Expression.Constant(convertedValue, property.Type);

            Expression comparison = criterion.Operator switch
            {
                "Equals" => Expression.Equal(property, constant),
                "Contains" when property.Type == typeof(string) =>
                    Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
                "GreaterThan" => Expression.GreaterThan(property, constant),
                "LessThan" => Expression.LessThan(property, constant),
                _ => throw new NotSupportedException("Unsupported operator")
            };

            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }
    }

}

