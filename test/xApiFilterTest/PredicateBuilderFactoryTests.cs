using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.ApiFilter;
using System.Text;
using xApiFilterTest.Db;
using Xunit;

namespace xApiFilterTest
{
    public class PredicateBuilderFactoryTests
    {
        [Fact]
        public void Create_With_StringPredicateBuilder()
        {
            var factory = new PredicateBuilderFactory();
            factory.AddBuilderType(typeof(string).FullName, typeof(StringPredicateBuilder<>));

            var filter = Filter.Parse("attributes.name:'test'").First();
            var builder = factory.Create<Model>(filter);

            Assert.IsType<StringPredicateBuilder<Model>>(builder);
        }
    }
}
