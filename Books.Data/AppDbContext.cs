using System;
using Books.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" },
                new Book { Id = Guid.NewGuid(), Title = "The Book Thief", Author = "Markus Zusak" },
                new Book { Id = Guid.NewGuid(), Title = "The Hobbit", Author = "J.R.R. Tolkien" },
                new Book { Id = Guid.NewGuid(), Title = "Fahrenheit 451", Author = "Ray Bradbury" },
                new Book { Id = Guid.NewGuid(), Title = "Animal Farm", Author = "George Orwell" });
        }
    }
}