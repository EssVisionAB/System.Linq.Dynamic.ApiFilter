using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.ApiFilter;
using System.Text;
using Xunit;

namespace xApiFilterTest
{
    public class FilterTests
    {
        [Fact]
        public void StringEqualFilter()
        {
            var filterString = "attributes.contact.name:'berit'";
            var filter = Filter.Parse(filterString).First();

            Assert.Equal("contact.name", filter.Name);
            Assert.Equal(Filter.Operands.Equal, filter.Operand);
            Assert.Equal("berit", filter.Values[0]);

        }

        [Fact]
        public void DateGreaterThanOrEqualFilter()
        {
            var filterString = "attributes.date>:'2017-01-01'";
            var filter = Filter.Parse(filterString).First();

            Assert.Equal("date", filter.Name);
            Assert.Equal(Filter.Operands.GreaterThanOrEqual, filter.Operand);
            Assert.Equal("2017-01-01", filter.Values[0]);
        }

        [Fact]
        public void InclusiveOrFilter()
        {
            var filterString = "attributes.name:('a','b')";
            var filter = Filter.Parse(filterString).First();

            Assert.Equal("name", filter.Name);
            Assert.Equal(Filter.Operands.InclusiveOrEqual, filter.Operand);
            Assert.Equal(2, filter.Values.Length);
            Assert.Equal("a", filter.Values[0]);
            Assert.Equal("b", filter.Values[1]);
        }

        [Fact]
        public void MissingOperandException()
        {
            var filterString = "attributes.date+'2017-01-01'";

            // Act
            var ex = Assert.Throws<DynamicFilterException>(() => Filter.Parse(filterString));

            Assert.Equal("Filtret saknar giltig operand.", ex.Message);

        }
    }
}
