using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using xApiFilterTest.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace xApiFilterTest
{
    public class NullFilterTests : IClassFixture<FilterProviderFixture>
    {
        class Model
        {
            public string Name { get; set; }
            public DateTime? ClosedDate { get; set; }
        }


        private readonly ITestOutputHelper _testOutput;
        private readonly FilterProviderFixture _filterProvider;

        public NullFilterTests(ITestOutputHelper testOutput, FilterProviderFixture filterProvider)
        {
            _testOutput = testOutput;
            _filterProvider = filterProvider;
        }

        [Fact]
        public void Experimental()
        {
            var propName = "ClosedDate";
            var propType = typeof(Model);
            var pi = propType.GetProperty(propName);
            var targetType = pi.PropertyType;

            Assert.Equal(typeof(DateTime?), targetType);

        }

        [Fact]
        public void SanityCheck()
        {
            var a = "A";
            var b = "B";

            var test = string.Format("{0}", a, b);

            Assert.Equal("A", test);
        }

        [Fact]
        public void ConvertTest1()
        {
            var strValue = "";
            var converter = TypeDescriptor.GetConverter(typeof(DateTime?));

            var result = converter.ConvertFromString(strValue);

            _testOutput.WriteLine((null != converter).ToString());
            _testOutput.WriteLine(converter.CanConvertFrom(typeof(string)).ToString());

            Assert.Null(result);
        }

        [Fact]
        public void FilterEntities_WhereDateTimeIsNull()
        {
            // Arrange
            var source = new List<Model>
            {
                new Model(),
                new Model{ClosedDate=DateTime.Now}
            }
            .AsQueryable();

            var filterString = "ClosedDate:"; // Empty string is converted to null

            // Act
            var provider = _filterProvider.Provider;
            var result = provider.ApplyFilter(source, filterString);

            // Assert
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void FilterEntities_WhereDateTimeIsNotNull()
        {
            // Arrange
            var source = new List<Model>
            {
                new Model(),
                new Model{ClosedDate=DateTime.Now}
            }
            .AsQueryable();

            var filterString = "ClosedDate<>"; // Empty string is converted to null

            // Act
            var provider = _filterProvider.Provider;
            var result = provider.ApplyFilter(source, filterString);

            // Assert
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void FilterEntities_TestNullOrEmptyString()
        {
            var source = new List<Model>
            {
                new Model(),
                new Model{ Name=""},
                new Model{Name="kalle"}
            }
            .AsQueryable();

            // filter for null or empty
            var filterString = "Name<>";

            // Act
            var provider = _filterProvider.Provider;
            var result = provider.ApplyFilter(source, filterString);

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}

