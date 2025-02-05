using Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AccountsConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
          .ValueGeneratedOnAdd()
          .HasDefaultValueSql("NEWID()");

        builder.HasIndex(a => a.Id).IsUnique();
        builder.Property(a => a.AccountName).IsRequired();
    }
}
