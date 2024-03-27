using Wallet.Domain.Entities.Model;

namespace Wallet.Domain.Contracts;

public interface IAccountRepository
{
    void CreateAccount(Account account);
    void DeleteAccount(Account account);
    void UpdateAccount(Account account);

    Task<Account?> GetAccountAsync(Guid accountId, bool trackChanges, CancellationToken cancellationToken);
    Task<Account?> GetAccountByNameAsync(string name);

    bool AccountExists(Guid accountId);
}