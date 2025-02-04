using Wallet.Domain.Contracts;

namespace Wallet.Infrastructure.Repository;

public class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<ITransactionRepository> _transactionRepository = new(() => new TransactionRepository(repositoryContext));
    private readonly Lazy<IAccountRepository> _accountRepository = new(() => new AccountRepository(repositoryContext));
    private readonly Lazy<IAccountTelegramsRepository> _accountTelegramsRepository = new(() => new AccountTelegramsRepository(repositoryContext));
    private readonly Lazy<IWalletIdentityUsersRepository> _walletIdentityUsersRepository = new(() => new WalletIdentityUsersRepository(repositoryContext));

    public ITransactionRepository Transaction => _transactionRepository.Value;
    public IAccountRepository Account => _accountRepository.Value;
    public IAccountTelegramsRepository AccountTelegrams => _accountTelegramsRepository.Value;
    public IWalletIdentityUsersRepository WalletIdentityUsers => _walletIdentityUsersRepository.Value;

    public Task SaveAsync(CancellationToken cancellationToken) { return repositoryContext.SaveChangesAsync(cancellationToken); }
}