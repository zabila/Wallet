using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email).IsRequired();

        // One-to-Many: A user belongs to one account, an account has many users
        builder.HasOne(u => u.Account)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-Many: A user has many transactions
        builder.HasMany(u => u.Transactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-One: A user has one TelegramUser
        builder.HasOne(u => u.TelegramUser)
            .WithOne(t => t.User)
            .HasForeignKey<TelegramUser>(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
