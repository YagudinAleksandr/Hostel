using Hostel.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        }


    }
}
