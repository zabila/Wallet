using Domain.Accounts;
using Domain.Transactions;
using Domain.Users;

namespace Application.Data;

public interface IRepositoryManager
{
    IRepositoryBase<Transaction> Transactions { get; }
    IRepositoryBase<User> Users { get; }
    IRepositoryBase<Account> Accounts { get; }
    IRepositoryBase<TelegramUser> TelegramUsers { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
