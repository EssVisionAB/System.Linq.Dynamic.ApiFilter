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
                var targetType = f.GetTargetType<TEntity>();
                var values = f.Values.ConvertFromStringValues(targetType);

                var builder = _builderFactory.Create<TEntity>(f);
                var predicate = builder.Build(values);

                query = query.Where(predicate, values);
            }

            return query;
        }
    }
}
