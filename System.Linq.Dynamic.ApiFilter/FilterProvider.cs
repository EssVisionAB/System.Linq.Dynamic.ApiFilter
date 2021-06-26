using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace System.Linq.Dynamic.ApiFilter
{
    public class FilterProvider : IFilterProvider
    {
        private IPredicateBuilderFactory _builderFactory;

        public FilterProvider(IPredicateBuilderFactory builderFactory)
        {
            _builderFactory = builderFactory ?? throw new ArgumentNullException(nameof(builderFactory));
        }

        public IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, string filterString)
        {
            if (string.IsNullOrEmpty(filterString))
            {
                return query;
            }

            var filters = Filter.Parse(filterString);
            return ApplyFilter(query, filters);
        }

        public IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, IEnumerable<Filter> filters)
        {
            if (null == filters)
            {
                return query;
            }

            foreach (var f in filters)
            {
                if(null != f.OrFilters)
                {
                    var predicates = new List<string>();
                    object[] values = new object[0];
                    foreach(var f2 in f.OrFilters)
                    {
                        var targetType = f2.GetTargetType<TEntity>();
                        values = f2.Values.ConvertFromStringValues(targetType);

                        var builder = _builderFactory.Create<TEntity>(f2);
                        predicates.Add(builder.Build(values));
                    }

                    var predicate = string.Join(" || ", predicates);
                    query = query.Where(predicate, values);
                }
                else
                {
                    var targetType = f.GetTargetType<TEntity>();
                    var values = f.Values.ConvertFromStringValues(targetType);

                    var builder = _builderFactory.Create<TEntity>(f);
                    var predicate = builder.Build(values);

                    query = query.Where(predicate, values);
                }

            }

            return query;
        }
    }
}
