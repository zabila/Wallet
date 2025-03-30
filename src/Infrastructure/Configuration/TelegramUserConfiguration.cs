using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TelegramUserConfiguration : IEntityTypeConfiguration<TelegramUser>
{
    public void Configure(EntityTypeBuilder<TelegramUser> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(t => t.Id).IsUnique();
        builder.HasIndex(t => t.TelegramUserId).IsUnique();
        builder.Property(t => t.TelegramUserId).IsRequired();
        builder.Property(t => t.UserId).IsRequired();

        // One-to-One: A TelegramUser belongs to one User, and a User has one TelegramUser
        builder.HasOne(t => t.User)
            .WithOne(u => u.TelegramUser)
            .HasForeignKey<TelegramUser>(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
