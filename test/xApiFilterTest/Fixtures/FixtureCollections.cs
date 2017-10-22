using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace xApiFilterTest.Fixtures
{
    [CollectionDefinition("FixtureCollections")]
    public class FixtureCollections : 
        ICollectionFixture<ModelDbContextFixture>, 
        ICollectionFixture<FilterProviderFixture>
    {
    }
}

