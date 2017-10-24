using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.ApiFilter;
using System.Text;
using Xunit;

namespace xApiFilterTest
{
    [Collection("FixtureCollections")]
    public class FilterProviderTests
    {
        private readonly DbContextOptions<Db.ModelDbContext> _dbOptions;
        private readonly IFilterProvider _filterProvider;

        public FilterProviderTests(
            Fixtures.ModelDbContextFixture dbFixture,
            Fixtures.FilterProviderFixture filterFixture
            )
        {
            _dbOptions = dbFixture.Options;
            _filterProvider = filterFixture.Provider;
        }

        [Fact]
        public void Assert_DbContext()
        {
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var models = db.Models.ToArray();
                var users = db.Users.ToArray();
                var contacts = db.Contacts.ToArray();

                Assert.Equal(3, models.Length);
                Assert.Equal(2, users.Length);
                Assert.Equal(3, contacts.Length);
            }
        }

        [Fact]
        public void FilterBy_Name_InclusiveOrEqual()
        {
            // NOTE: we are using in-memory-db so it's case sensitive
            var filter = "x.name:(AAA,BBB,CCC)";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);

                var result = q.ToArray();

                Assert.NotNull(result);
                Assert.Equal(3, result.Length);
            }
        }

        [Fact]
        public void FilterBy_Name_InclusiveOrLike()
        {
            // NOTE: we are using in-memory-db so it's case sensitive
            var filter = "x.name~(A,B,C)";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);

                var result = q.ToArray();

                Assert.NotNull(result);
                Assert.Equal(3, result.Length);
            }
        }


        [Fact]
        public void FilterBy_Contacts_Like()
        {
            // NOTE: we are using in-memory-db so it's case sensitive
            var filter = "x.contacts.name~Gud";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);

                var result = q.ToArray();

                Assert.NotNull(result);
                Assert.Equal(1, result.Length);
                Assert.Equal("BBB", result.First().Name);
            }
        }

        [Fact]
        public void FilterBy_Responsible_Equal_Like()
        {
            // NOTE: we are using in-memory-db so it's case sensitive
            var filter = "x.responsible.name:Kalle;x.responsible.email~com";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);

                var result = q.ToArray();

                Assert.NotNull(result);
                Assert.Equal(1, result.Length);
                Assert.Equal("AAA", result.First().Name);
            }
        }

        [Fact]
        public void FilterBy_Responsible_InclusiveOr()
        {
            // NOTE: we are using in-memory-db so it's case sensitive
            var filter = "x.responsible.name:(Kalle, Olle)";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);

                var result = q.ToArray();

                Assert.NotNull(result);
                Assert.Equal(2, result.Length);
            }
        }

        [Fact]
        public void FilterBy_RegisteredDate_GreaterThanOrEqual()
        {
            var filter = "x.registereddate>:2016-12-30";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);

                var result = q.ToArray();

                Assert.NotNull(result);
                Assert.Equal(1, result.Length);
                Assert.Equal("AAA", result.First().Name);
            }
        }

        //[Fact]
        public void FilterBy_ResponsibleIsNull()
        {
            // TODO: handle filter on null values
            var filter = "x.responsible:null";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                q = _filterProvider.ApplyFilter(q, filter);
            }
        }


        [Fact]
        public void InvalidFilterAttributeException()
        {
            var filter = "x.description~test";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                // Act
                var ex = Assert.Throws<DynamicFilterException>(() => _filterProvider.ApplyFilter(q, filter));

                Assert.Equal("Property 'description' is not a member of the taget entity.", ex.Message);
            }
        }

        [Fact]
        public void OperandForAttributeNotSupportedException()
        {
            var filter = "x.name>:test";
            using (var db = new Db.ModelDbContext(_dbOptions))
            {
                var q = db.Models.AsQueryable();
                // Act
                var ex = Assert.Throws<DynamicFilterException>(() => _filterProvider.ApplyFilter(q, filter));

                Assert.Equal("The operand '>:' is not supported for property 'name'.", ex.Message);
            }
        }
    }
}
