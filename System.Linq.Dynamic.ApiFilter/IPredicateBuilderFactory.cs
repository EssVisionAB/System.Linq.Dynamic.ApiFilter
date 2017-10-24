using System.Collections.Generic;

namespace System.Linq.Dynamic.ApiFilter
{
    public interface IPredicateBuilderFactory
    {
        IPredicateBuilder Create<TEntity>(Filter filter);
    }
}