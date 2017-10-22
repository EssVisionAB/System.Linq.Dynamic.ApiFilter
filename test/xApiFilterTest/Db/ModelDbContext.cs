using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace xApiFilterTest.Db
{
    public class ModelDbContext : DbContext
    {
        public ModelDbContext() { }

        public ModelDbContext(DbContextOptions<ModelDbContext> options)
            : base(options)
        { }


        public DbSet<Model> Models { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .HasOne(x => x.Model)
                .WithMany(x => x.Contacts)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseInMemoryDatabase("")
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
            }
        }

    }
}
