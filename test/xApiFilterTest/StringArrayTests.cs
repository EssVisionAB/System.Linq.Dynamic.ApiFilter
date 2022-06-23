using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.ApiFilter;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xApiFilterTest
{
    [Collection("FixtureCollections")]
    public class StringArrayTests
    {
        private readonly IFilterProvider _filterProvider;

        public StringArrayTests(
            Fixtures.ModelDbContextFixture dbFixture,
            Fixtures.FilterProviderFixture filterFixture
            )
        {
            _filterProvider = filterFixture.Provider;
        }

        [Fact]
        public void ShouldDoExactMatch()
        {
            var filter = "Values:Kalle Anka";

            var q = _filterProvider.ApplyFilter(Source, filter);

            var result = q.ToArray();

            Assert.Single(result);
        }

        [Fact]
        public void ShouldDoLikeMatch()
        {
            var filter = "Values~Kalle";

            var q = _filterProvider.ApplyFilter(Source, filter);

            var result = q.ToArray();

            Assert.Equal(3, result.Length);
        }

        static IQueryable<Model> Source = new List<Model>()
            {
                new Model(new string[] {"Kalle Anka"}),
                new Model(new string[] {"Olle"}),
                new Model(new string[] {"Pelle"}),
                new Model(new string[] {"Kalle"}),
                new Model(new string[] {"Anka", "Kalle"}),
            }.AsQueryable();

        class Model
        {
            public Model()
            {
                Values = new HashSet<string>();
            }

            public Model(IEnumerable<string> values)
            {
                Values = new HashSet<string>(values);
            }

            public ICollection<string> Values { get; set; }
        }
    }
}
