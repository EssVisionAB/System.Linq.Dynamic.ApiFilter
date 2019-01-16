using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace xApiFilterTest.Fixtures
{
    public class ModelDbContextFixture
    {
        public ModelDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<Db.ModelDbContext>()
                  .UseInMemoryDatabase("In mem db")
                  .Options;

            using (var db = new Db.ModelDbContext(options))
            {
                db.Models.Add(new Db.Model { Id = 1, Name = "AAA", RegisteredDate = DateTime.Parse("2017-01-01"), ResponsibleId = 1 });
                db.Models.Add(new Db.Model { Id = 2, Name = "BBB", ResponsibleId = 2, Extension = ".docx" });
                db.Models.Add(new Db.Model { Id = 3, Name = "CCC", Extension = ".doc" });

                db.Users.Add(new Db.User { Id = 1, Name = "Kalle", Email = "kalle@com.se" });
                db.Users.Add(new Db.User { Id = 2, Name = "Olle", Email = "olle@com.se" });

                db.Contacts.Add(new Db.Contact { Id = 1, ModelId = 1, Name = "Rosanna", Address = "Mars" });
                db.Contacts.Add(new Db.Contact { Id = 2, ModelId = 2, Name = "Margot", Address = "Venus" });
                db.Contacts.Add(new Db.Contact { Id = 3, ModelId = 2, Name = "Gudrun", Address = "Moon" });

                db.SaveChanges();
            }

            Options = options;
        }

        public DbContextOptions<Db.ModelDbContext> Options { get; }
    }
}
