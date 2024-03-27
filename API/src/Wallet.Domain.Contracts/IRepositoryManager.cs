namespace Wallet.Domain.Contracts;

public interface IRepositoryManager
{
    ITransactionRepository Transaction { get; }
    IAccountRepository Account { get; }

    IAccountTelegramsRepository AccountTelegrams { get; }

    Task SaveAsync(CancellationToken cancellationToken);
}