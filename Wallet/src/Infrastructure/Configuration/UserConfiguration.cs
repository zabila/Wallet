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
          .HasDefaultValueSql("NEWID()");

        builder.HasIndex(u => u.Id).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email).IsRequired();
    }
}
