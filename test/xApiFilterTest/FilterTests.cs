using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.ApiFilter;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace xApiFilterTest
{
    public class FilterTests
    {
        private readonly ITestOutputHelper _testOutput;

        public FilterTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void StringEqualFilter()
        {
            var filterString = "contact.name:'berit'";
            var filter = Filter.Parse(filterString).First();

            Assert.Equal("contact.name", filter.Name);
            Assert.Equal(Filter.Operands.Equal, filter.Operand);
            Assert.Equal("berit", filter.Values[0]);

        }

        [Fact]
        public void DateGreaterThanOrEqualFilter()
        {
            var filterString = "date>:'2017-01-01'";
            var filter = Filter.Parse(filterString).First();

            Assert.Equal("date", filter.Name);
            Assert.Equal(Filter.Operands.GreaterThanOrEqual, filter.Operand);
            Assert.Equal("2017-01-01", filter.Values[0]);
        }

        [Fact]
        public void InclusiveFilter()
        {
            var filterString = "name:('a','b')";
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
            var filterString = "date+'2017-01-01'";

            // Act
            var ex = Assert.Throws<DynamicFilterException>(() => Filter.Parse(filterString));

            Assert.Equal("Supplied operand is not supported.", ex.Message);

        }

        [Fact]
        public void Lab()
        {
            var value = "berit";
            var filterString = $"contact.name~'{value}'|name~'{value}";

            var result = new List<string>();
            var andPredicates = filterString.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach(var predicate in andPredicates)
            {
                var temp = predicate.Split('|', StringSplitOptions.RemoveEmptyEntries);
                var orPredicate = string.Join(" || ", temp);
                result.Add(orPredicate);                        
            }

            foreach(var r in result)
            {
                _testOutput.WriteLine(r);
            }
        }

    }
}
