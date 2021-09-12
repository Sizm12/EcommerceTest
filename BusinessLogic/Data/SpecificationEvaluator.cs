using Core.Entities;
using Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class SpecificationEvaluator<T> where T : ClaseBase
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputquery, ISpecification<T> spec)
        {
            if (spec.Criteria != null)
            {
                inputquery = inputquery.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                inputquery = inputquery.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                inputquery = inputquery.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnable)
            {
                inputquery = inputquery.Skip(spec.Skip).Take(spec.Take);
            }

            inputquery = spec.Includes.Aggregate(inputquery, (current, include) => current.Include(include));
            return inputquery;
        }
    }
}
