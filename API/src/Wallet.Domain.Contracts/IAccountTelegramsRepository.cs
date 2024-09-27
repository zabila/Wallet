using Wallet.Domain.Entities.Model;

namespace Wallet.Domain.Contracts;

public interface IAccountTelegramsRepository {
    void CreateAccountTelegram(AccountTelegram accountTelegram);
    void DeleteAccountTelegram(AccountTelegram accountTelegram);
    void UpdateAccountTelegram(AccountTelegram accountTelegram);

    Task<Guid> GetAccountIdByTelegramUserIdAsync(int telegramUserId, bool trackChanges, CancellationToken cancellationToken);
    bool AccountTelegramExists(AccountTelegram accountTelegram);
}