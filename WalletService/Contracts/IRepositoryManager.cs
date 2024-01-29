namespace Contracts;

public interface IRepositoryManager
{
    ITransactionRepository Transaction { get; }
    IAccountRepository Account { get; }

    Task SaveAsync(CancellationToken cancellationToken = new CancellationToken());
}