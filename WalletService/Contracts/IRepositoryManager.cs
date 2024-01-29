namespace Contracts;

public interface IRepositoryManager
{
    ITransactionRepository Transaction { get; }

    Task SaveAsync(CancellationToken cancellationToken = new CancellationToken());
}