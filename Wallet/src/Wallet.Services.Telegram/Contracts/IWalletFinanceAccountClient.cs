using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Services.Telegram.Contracts;

public interface IWalletFinanceAccountClient
{
    Task TestInboundConnectionAsync();
    Task<List<string>> GetIncomingCategoriesAsync();
    Task<List<string>> GetOutcomingCategoriesAsync();

    Task<AccountReadDto?> GetAccountIdByTelegramUserIdAsync(int telegramUserId);
}