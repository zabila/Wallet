using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.Model;
using Wallet.Infrastructure.Repository.Configuration;

namespace Wallet.Infrastructure.Repository;

public class RepositoryContext(DbContextOptions options) : IdentityDbContext<WalletIdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Wallet");
        modelBuilder.Entity<WalletIdentityUser>(entity => { entity.ToTable(name: "Users"); });
        modelBuilder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
        modelBuilder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
        modelBuilder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
        modelBuilder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
        modelBuilder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.OwnsOne(t => t.Location, locationConfig =>
            {
                locationConfig.Property(l => l.Longitude)
                    .HasColumnName("Longitude")
                    .HasColumnType("decimal(9,6)");

                locationConfig.Property(l => l.Latitude)
                    .HasColumnName("Latitude")
                    .HasColumnType("decimal(9,6)");
            });
        });

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }

    public DbSet<Transaction>? Transactions { get; set; }
    public DbSet<Account>? Accounts { get; set; }
    public DbSet<AccountTelegram>? AccountTelegrams { get; set; }
    public DbSet<WalletIdentityUser>? WalletIdentityUsers { get; set; }
}