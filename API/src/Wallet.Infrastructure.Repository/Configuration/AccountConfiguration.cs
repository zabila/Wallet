using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities.Model;

namespace Wallet.Infrastructure.Repository.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(a => a.AccountName).IsRequired();
        builder.HasIndex(a => a.AccountName).IsUnique();
    }
}