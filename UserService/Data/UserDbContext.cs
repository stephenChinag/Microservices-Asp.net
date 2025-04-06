// UserService/Data/UserDbContext.cs
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add some seed data for testing
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$ej7Oi5k/ZzpTnr5Ws8yp8.QOufKwdTqQH5KZF/QmCU3CGARvxzNrC", // Pre-computed hash for "admin123"
                    Email = "admin@example.com",
                    IsAdmin = true,
                    CreatedAt = new DateTime(2025, 4, 1) // Fixed date
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    PasswordHash = "$2a$11$I3MmunU0JwNUowvKmp.BCOA9h5GciUDSTaLqFzlYVjkJrGssvRmBK", // Pre-computed hash for "user123"
                    Email = "user@example.com",
                    IsAdmin = false,
                    CreatedAt = new DateTime(2025, 4, 1) // Fixed date
                }
            );
        }


    }
}