using Domain.Accounts;
using Domain.Transactions;
using Domain.Users;
using Infrastructure.Abstractions;
using Infrastructure.Configuration;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure;

public sealed class RepositoryContext(DbContextOptions<RepositoryContext> options, IPublisher publisher) : IdentityDbContext(options), IRepositoryContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<TelegramUser> TelegramUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Wallet");
        builder.ApplyConfiguration(new AccountsConfiguration());
        builder.ApplyConfiguration(new TransactionsConfiguration());
        builder.ApplyConfiguration(new TelegramUserConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync();
        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
