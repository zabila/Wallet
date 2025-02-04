using SharedKernel.DTO.Users;

namespace API.Telegram.Contracts;

public interface IWalletIdentityClient
{
    Task TestInboundConnectionAsync();
    Task<UserResponse> GetCurrentUserByTelegramUserIdAsync(long telegramUserId);
}
