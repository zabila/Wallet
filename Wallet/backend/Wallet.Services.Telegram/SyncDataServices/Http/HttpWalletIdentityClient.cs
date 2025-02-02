using System.Text;
using Flurl.Http;
using Flurl.Http.Configuration;
using Wallet.Services.Telegram.Contracts;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.SyncDataServices.Http;

public class HttpWalletIdentityClient(ILoggerManager logger, IFlurlClientCache clients) : HttpClientBase(logger, clients.Get(nameof(HttpWalletIdentityClient)).EnsureExists()), IWalletIdentityClient {
    public Task TestInboundConnectionAsync() {
        return PostAsync<string>("test");
    }
}