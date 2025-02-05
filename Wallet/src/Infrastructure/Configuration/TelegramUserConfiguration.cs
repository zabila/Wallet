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
          .HasDefaultValueSql("NEWID()");

        builder.HasIndex(u => u.Id).IsUnique();
        builder.Property(u => u.UserId).IsRequired();
    }
}
