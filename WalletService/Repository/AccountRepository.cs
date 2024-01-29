using Contracts;
using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public sealed class AccountRepository(DbContext repositoryContext) : RepositoryBase<Account>(repositoryContext), IAccountRepository
{
    public void CreateAccount(Account account) => Create(account);
    public void DeleteAccount(Account account) => Delete(account);
    public void UpdateAccount(Account account) => Update(account);

    public async Task<Account?> GetAccountAsync(Guid accountId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(account => account.Id.Equals(accountId), trackChanges).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Account?> GetAccountByNameAsync(string name)
    {
        return await FindByCondition(account => account.AccountName!.Equals(name), false).SingleOrDefaultAsync();
    }

    public bool AccountExists(Guid accountId) => FindByCondition(account => account.Id.Equals(accountId), false).Any();
}