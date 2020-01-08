using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Domain
{
    public class LibraryDataContext : DbContext
    {
        public LibraryDataContext(DbContextOptions<LibraryDataContext> ctx): base(ctx)
        {

        }
        public DbSet<Book> Books { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                    new Book { Id = 1, Title = "Walden", Author = "Thoreau", Genre = "Philosiphy" },
                    new Book { Id = 2, Title = "Nature", Author = "Emerson", Genre = "Philosiphy" }
                );
            modelBuilder.Entity<Book>().Property(p => p.Author).HasMaxLength(200);
        }
    }
}
