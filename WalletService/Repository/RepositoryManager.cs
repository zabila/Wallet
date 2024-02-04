using Contracts;

namespace Repository;

public class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<ITransactionRepository> _transactionRepository = new(() => new TransactionRepository(repositoryContext));
    private readonly Lazy<IAccountRepository> _accountRepository = new(() => new AccountRepository(repositoryContext));
    private readonly Lazy<IAccountTelegramsRepository> _accountTelegramsRepository = new(() => new AccountTelegramsRepository(repositoryContext));

    public ITransactionRepository Transaction => _transactionRepository.Value;
    public IAccountRepository Account => _accountRepository.Value;
    public IAccountTelegramsRepository AccountTelegrams => _accountTelegramsRepository.Value;

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await repositoryContext.SaveChangesAsync(cancellationToken);
    }
}