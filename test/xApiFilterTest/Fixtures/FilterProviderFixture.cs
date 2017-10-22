using System;
using System.Collections.Generic;
using System.Linq.Dynamic.ApiFilter;
using System.Text;

namespace xApiFilterTest.Fixtures
{
    public class FilterProviderFixture
    {
        public FilterProviderFixture()
        {
            var builderFactory = new PredicateBuilderFactory();
            var provider = new FilterProvider(builderFactory);
            Provider = provider;
        }

        public IFilterProvider Provider { get; }
    }
}
