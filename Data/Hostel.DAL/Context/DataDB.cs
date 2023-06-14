using Hostel.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hostel.DAL.Context
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class DataDB : IdentityDbContext<UserEntity>
    {
        public DataDB(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<UserEntity>();

            modelBuilder.Entity<UserEntity>()
                .HasData(new UserEntity
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "admin@supply.ru",
                    Email = "admin@supply.ru",
                    NormalizedEmail = "admin@supply.ru",
                    IsActive = true,
                    IsAdmin = true,
                    NormalizedUserName = "Администратор",
                    Fullname = "Администратор",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    PasswordHash = hasher.HashPassword(null, "password651"),
                    EmailConfirmed = true,
                });
        }


    }
}
