using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.Helpers
{
    public static class SortingExtension
    {
        public static IQueryable Sort(this IQueryable collection, string sortBy, string sortDirection)
        {
            bool reverse = false;

            if (sortDirection.ToLower() == "descending" || sortDirection.ToLower() == "desc")
            {
                reverse = true;
            }

            return collection.OrderBy(sortBy +  (reverse ? " descending" : ""));
        }
    }
}
