using Wallet.Shared.DataTransferObjects;

namespace Wallet.Services.Telegram.Contracts;

public interface IWalletIdentityClient {
    Task TestInboundConnectionAsync();
    Task<CurrentUserDto> GetCurrentUserByTelegramUserIdAsync(int telegramUserId);
}