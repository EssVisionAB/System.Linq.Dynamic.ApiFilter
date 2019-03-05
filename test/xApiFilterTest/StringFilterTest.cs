using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.ApiFilter;
using System.Text;
using xApiFilterTest.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace xApiFilterTest
{
    public class StringFilterTest : IClassFixture<FilterProviderFixture>
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly IFilterProvider _filterProvider;

        public StringFilterTest(ITestOutputHelper testOutput, FilterProviderFixture filterProvider)
        {
            _testOutput = testOutput;
            _filterProvider = filterProvider.Provider;
        }

        [Fact]
        public void Should_Return_Inclusive_Or_Like()
        {
            var filter = "Keywords~('kalle', 'olle')";
            var actual = _filterProvider.ApplyFilter(Data, filter).ToArray();

            Assert.Equal(3, actual.Length);
        }

        [Fact]
        public void Should_Return_Inclusive_And_Like()
        {
            var filter = "Keywords~:('kalle', 'olle')";
            var actual = _filterProvider.ApplyFilter(Data, filter).ToArray();

            Assert.Equal(2, actual.Length);
        }


        class MyModel
        {
            public string Keywords { get; set; }
        }

        static IQueryable<MyModel> _data;
        static IQueryable<MyModel> Data
        {
            get
            {
                if (null == _data)
                {
                    _data = new List<MyModel>
                    {
                        new MyModel{ Keywords = "kalle;olle;pelle" },
                        new MyModel{ Keywords = "kalle;olle" },
                        new MyModel{ Keywords = "kalle" },
                    }.AsQueryable();
                }
                return _data;
            }
        }
    }
}

