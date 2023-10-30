using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace ChatKid.Common.Extensions
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> query, string sortBy)
        {
            string[] sorts = sortBy.Split(",");
            var properties = typeof(T).GetProperties();
            string orderParam = String.Join(", ", sorts.AsEnumerable().Select(sort =>
            {
                foreach (var property in properties)
                {
                    if (property.Name.Equals(sort.Substring(1), StringComparison.OrdinalIgnoreCase))
                    {
                        return sort[0] == '-' ? property.Name + " desc" : property.Name + " asc";
                    }
                }
                return null;
            }));
            return query.OrderBy(orderParam);
        }

        public static bool IsAcceptSort<T>(this string sortBy)
        {
            string[] sorts = sortBy.Split(",");
            var properties = typeof(T).GetProperties();
            int count = 0;
            foreach (var sort in sorts)
            {
                foreach (var property in properties)
                {
                    if (property.Name.Equals(sort.Substring(1), StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
                    }
                }
            }
            return (count == sorts.Length);
        }
    }
}
