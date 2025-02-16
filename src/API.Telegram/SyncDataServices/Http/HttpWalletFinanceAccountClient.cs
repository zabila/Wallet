using API.Telegram.Contracts;
using Flurl.Http.Configuration;
using SharedKernel.Abstractions;
using SharedKernel.Extensions;

namespace API.Telegram.SyncDataServices.Http;

public class HttpWalletFinanceAccountClient(ILoggerManager logger, IWalletIdentityClient identityClient, IFlurlClientCache clients) : HttpClientBase(logger, clients.Get(nameof(HttpWalletFinanceAccountClient)).EnsureExists()), IWalletFinanceAccountClient
{
    public Task TestInboundConnectionAsync() { return identityClient.TestInboundConnectionAsync(); }

    public Task<List<string>> GetIncomingCategoriesAsync()
    {
        return Task.FromResult(new List<string> {
            "Salary",
            "Gift",
            "Other",
            "Refund",
            "Loan",
            "Investment",
            "Savings"
        });
    }

    public Task<List<string>> GetOutcomingCategoriesAsync()
    {
        return Task.FromResult(new List<string> {
            "Food",
            "Transport",
            "Other"
        });
    }
}
