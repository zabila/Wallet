using Contracts;
using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class AccountTelegramsRepository(DbContext repositoryContext) : RepositoryBase<AccountTelegram>(repositoryContext), IAccountTelegramsRepository
{
    public void CreateAccountTelegram(AccountTelegram accountTelegram)
    {
        Create(accountTelegram);
    }

    public void DeleteAccountTelegram(AccountTelegram accountTelegram)
    {
        Delete(accountTelegram);
    }

    public void UpdateAccountTelegram(AccountTelegram accountTelegram)
    {
        Update(accountTelegram);
    }

    public async Task<Guid> GetAccountIdByTelegramUserIdAsync(int telegramUserId, bool trackChanges, CancellationToken cancellationToken)
    {
        return await FindByCondition(a => a.TelegramUserId.Equals(telegramUserId), trackChanges).Select(a => a.AccountId).FirstOrDefaultAsync(cancellationToken);
    }

    public bool AccountTelegramExists(AccountTelegram accountTelegram)
    {
        return FindByCondition(a => a.AccountId.Equals(accountTelegram.AccountId) && a.TelegramUserId.Equals(accountTelegram.TelegramUserId), false).Any();
    }
}