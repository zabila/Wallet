using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Model;

namespace Wallet.Infrastructure.Repository;

public sealed class AccountRepository(DbContext repositoryContext) : RepositoryBase<Account>(repositoryContext), IAccountRepository {
    public void CreateAccount(Account account) => Create(account);
    public void DeleteAccount(Account account) => Delete(account);
    public void UpdateAccount(Account account) => Update(account);

    public Task<Account?> GetAccountAsync(Guid accountId, bool trackChanges, CancellationToken cancellationToken) { return FindByCondition(account => account.Id.Equals(accountId), trackChanges).SingleOrDefaultAsync(cancellationToken); }

    public Task<Account?> GetAccountByNameAsync(string name) { return FindByCondition(account => account.AccountName!.Equals(name), false).SingleOrDefaultAsync(); }

    public bool AccountExists(Guid accountId) => FindByCondition(account => account.Id.Equals(accountId), false).Any();
}