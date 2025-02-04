using Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TransactionsConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
          .ValueGeneratedOnAdd()
          .HasDefaultValueSql("gen_random_uuid()");

        builder.HasIndex(t => t.Id).IsUnique();
        builder.Property(t => t.UserId).IsRequired();
        builder.OwnsOne(t => t.Location, locationConfig =>
        {
            locationConfig.Property(l => l.Longitude)
                .HasColumnName("Longitude")
                .HasColumnType("decimal(9,6)");

            locationConfig.Property(l => l.Latitude)
                .HasColumnName("Latitude")
                .HasColumnType("decimal(9,6)");
        });

        builder.HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .IsRequired();

        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .IsRequired();
    }
}
