using System;
using BooksApi.Data;
using BooksApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksApi
{
    public class DataSeeder
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                context.Books.AddRange(
                    new Book {Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald"},
                    new Book {Id = Guid.NewGuid(), Title = "The Book Thief", Author = "Markus Zusak"},
                    new Book {Id = Guid.NewGuid(), Title = "The Hobbit", Author = "J.R.R. Tolkien"},
                    new Book {Id = Guid.NewGuid(), Title = "Fahrenheit 451", Author = "Ray Bradbury"},
                    new Book {Id = Guid.NewGuid(), Title = "Animal Farm", Author = "George Orwell"}
                );

                context.SaveChanges();
            }
        }
    }
}