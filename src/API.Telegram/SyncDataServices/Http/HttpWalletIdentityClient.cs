using API.Telegram.Contracts;
using Flurl.Http.Configuration;
using SharedKernel.Abstractions;
using SharedKernel.DTO.Users;
using SharedKernel.Extensions;

namespace API.Telegram.SyncDataServices.Http;

public class HttpWalletIdentityClient(ILoggerManager logger, IFlurlClientCache clients) : HttpClientBase(logger, clients.Get(nameof(HttpWalletIdentityClient)).EnsureExists()), IWalletIdentityClient
{
    public Task TestInboundConnectionAsync()
    {
        return PostAsync<string>("authentication/test");
    }

    public Task<UserResponse> GetCurrentUserByTelegramUserIdAsync(long userId)
    {
        return GetAsync<UserResponse>($"user/telegram/{userId}");
    }
}
