using Microsoft.EntityFrameworkCore;
using CMCS.Models;

namespace CMCS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>().ToTable("Claims");
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Claim>()
                .Property(c => c.TotalAmount)
                .HasColumnType("NUMERIC");

            modelBuilder.Entity<Claim>()
                .Property(c => c.DateProcessed)
                .HasColumnName("DateProcessed")
                .HasColumnType("DATETIME");

            modelBuilder.Entity<Claim>().ToTable("Claims", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<User>().ToTable("Users", t => t.ExcludeFromMigrations());
        }
    }
}