using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities.Model;
using Wallet.Infrastructure.Repository.Configuration;

namespace Wallet.Infrastructure.Repository;

public class RepositoryContext(DbContextOptions options) : IdentityDbContext<WalletIdentityUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Wallet");
        builder.Entity<WalletIdentityUser>(entity => entity.ToTable(name: "Users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));

        builder.Entity<Transaction>(entity =>
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

        //builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new AccountConfiguration());
    }

    public DbSet<Transaction>? Transactions { get; set; }
    public DbSet<Account>? Accounts { get; set; }
    public DbSet<AccountTelegram>? AccountTelegrams { get; set; }
    public DbSet<WalletIdentityUser>? WalletIdentityUsers { get; set; }
}
