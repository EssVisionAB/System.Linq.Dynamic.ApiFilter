using System.Collections.Generic;

namespace System.Linq.Dynamic.ApiFilter
{
    public interface IFilterProvider
    {
        IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, string filters);

        IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query, IEnumerable<Filter> filters);
    }
}
