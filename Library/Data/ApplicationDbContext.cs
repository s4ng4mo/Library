using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using System.Collections.Generic;

namespace Library.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define relationships
            builder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed the Genre table
            builder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fiction" },
                new Genre { Id = 2, Name = "Non-Fiction" },
                new Genre { Id = 3, Name = "Science Fiction" },
                new Genre { Id = 4, Name = "Fantasy" },
                new Genre { Id = 5, Name = "Mystery" },
                new Genre { Id = 6, Name = "Thriller" },
                new Genre { Id = 7, Name = "Romance" },
                new Genre { Id = 8, Name = "Biography" },
                new Genre { Id = 9, Name = "History" },
                new Genre { Id = 10, Name = "Science" },
                new Genre { Id = 11, Name = "Computer Science" },
                new Genre { Id = 12, Name = "Poetry" },
                new Genre { Id = 13, Name = "Drama" },
                new Genre { Id = 14, Name = "Children" },
                new Genre { Id = 15, Name = "Self-Help" },
                new Genre { Id = 16, Name = "Travel" },
                new Genre { Id = 17, Name = "Cooking" },
                new Genre { Id = 18, Name = "Art" },
                new Genre { Id = 19, Name = "Music" },
                new Genre { Id = 20, Name = "Sports" },
                new Genre { Id = 21, Name = "Philosophy" },
                new Genre { Id = 22, Name = "Psychology" },
                new Genre { Id = 23, Name = "Religion" },
                new Genre { Id = 24, Name = "Other" }
            );
        }
    }
}