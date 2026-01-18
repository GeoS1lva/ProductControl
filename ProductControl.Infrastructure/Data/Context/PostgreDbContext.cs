using Microsoft.EntityFrameworkCore;
using ProductControl.Domain.Entities;

namespace ProductControl.Infrastructure.Data.Context
{
    public class PostgreDbContext(DbContextOptions<PostgreDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(u =>
            {
                u.HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<Address>(a => a.UserId)
                .IsRequired();

                u.HasMany(u => u.StockMovements)
                .WithOne()
                .HasForeignKey(sm => sm.UserId)
                .IsRequired();

                u.OwnsOne(u => u.Password, password =>
                {
                    password.Property(p => p.HashPassword)
                    .HasColumnName("Hash")
                    .IsRequired();

                    password.Property(p => p.SaltPassword)
                    .HasColumnName("Salt")
                    .IsRequired();
                });
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.Property(p => p.Price).HasPrecision(18, 2);

                p.HasMany(p => p.StockMovement)
                .WithOne()
                .HasForeignKey(sm => sm.ProductId)
                .IsRequired();
            });
        }
    }
}
