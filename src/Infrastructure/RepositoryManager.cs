using Application.Data;
using Domain.Accounts;
using Domain.Transactions;
using Domain.Users;

namespace Infrastructure;

public class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<IRepositoryBase<Account>> _accountRepository = new(() => new RepositoryBase<Account>(repositoryContext));
    private readonly Lazy<IRepositoryBase<TelegramUser>> _telegramUserRepository = new(() => new RepositoryBase<TelegramUser>(repositoryContext));
    private readonly Lazy<IRepositoryBase<Transaction>> _transactionRepository = new(() => new RepositoryBase<Transaction>(repositoryContext));
    private readonly Lazy<IRepositoryBase<User>> _userRepository = new(() => new RepositoryBase<User>(repositoryContext));

    public IRepositoryBase<Transaction> Transactions => _transactionRepository.Value;

    public IRepositoryBase<User> Users => _userRepository.Value;

    public IRepositoryBase<Account> Accounts => _accountRepository.Value;

    public IRepositoryBase<TelegramUser> TelegramUsers => _telegramUserRepository.Value;

    public Task SaveChangesAsync(CancellationToken cancellationToken) { return repositoryContext.SaveChangesAsync(cancellationToken); }
}
