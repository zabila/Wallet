using System.Net;
using System.Text;
using Flurl.Http;
using Flurl.Http.Configuration;
using Wallet.Domain.Contracts;
using Wallet.Services.Telegram.Contracts;
using Wallet.SharedKernel.DataTransferObjects;
using Wallet.SharedKernel.Extensions;

namespace Wallet.Services.Telegram.SyncDataServices.Http;

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

    public Task<AccountReadDto?> GetAccountIdByTelegramUserIdAsync(int telegramUserId)
    {
        return GetAsync<AccountReadDto?>($"telegram/{telegramUserId}");
    }
}
