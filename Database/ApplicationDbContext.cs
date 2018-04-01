using Domain.DataBaseModels;
using Domain.DataBaseModels.Goods;
using Domain.DataBaseModels.Identity;
using Domain.DataBaseModels.Logging;
using Domain.DataBaseModels.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.EntityFrameworkCore
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserRole, string>
    {
        public DbSet<ServerEvent> ServerLog { get; set; }

        public DbSet<JwtRefreshToken> JwtRefreshTokens { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<ShopMode> ShopModes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Product>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder
                .Entity<JwtRefreshToken>()
                .HasIndex(t => t.RefreshToken)
                .IsUnique();
        }
    }
}
