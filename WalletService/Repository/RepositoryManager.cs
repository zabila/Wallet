using Contracts;

namespace Repository;

public class RepositoryManager(RepositoryContext repositoryContext) : IRepositoryManager
{
    private readonly Lazy<ITransactionRepository> _transactionRepository = new(() => new TransactionRepository(repositoryContext));

    public ITransactionRepository Transaction => _transactionRepository.Value;
    public IAccountRepository Account => new AccountRepository(repositoryContext);

    public async Task SaveAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await repositoryContext.SaveChangesAsync(cancellationToken);
    }
}