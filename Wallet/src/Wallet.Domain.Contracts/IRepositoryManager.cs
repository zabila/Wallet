namespace Wallet.Domain.Contracts;

public interface IRepositoryManager
{
    ITransactionRepository Transaction { get; }
    IAccountRepository Account { get; }

    IAccountTelegramsRepository AccountTelegrams { get; }

    IWalletIdentityUsersRepository WalletIdentityUsers { get; }

    Task SaveAsync(CancellationToken cancellationToken);
}