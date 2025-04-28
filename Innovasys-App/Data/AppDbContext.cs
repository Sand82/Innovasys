using Innovasys_App.Data.Models;
using Microsoft.EntityFrameworkCore;

using static Innovasys_App.Data.Constants.GlobalConstants;

namespace Innovasys_App.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<User>()
                .Property(e => e.Website)
                .HasColumnType(MaxLength);

            modelBuilder.Entity<User>()
               .Property(e => e.Note)
               .HasColumnType(MaxLength);

            modelBuilder.Entity<Address>()
               .Property(p => p.Lat)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Address>()
               .Property(p => p.Lng)
               .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
