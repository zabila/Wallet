using System.Text;
using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.SyncDataServices.Http;

public class HttpWalletDataClient(HttpClient httpClient, IConfiguration configuration, ILoggerManager logger) : IWalletDataClient {
    public async Task TestInboundConnection() {
        var httpContent = new StringContent("",
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.PostAsync($"{configuration["Wallet.API"]}", httpContent);

        if (response.IsSuccessStatusCode) {
            logger.LogInfo("Sync POST to Wallet.API was OK!");
        } else {
            logger.LogError("Sync POST to Wallet.API was NOT OK!");
        }
    }

    public Task<List<string>> GetIncomingCategoriesAsync() {
        return Task.FromResult(new List<string> {
            "Salary",
            "Gift",
            "Other",
            "Refund",
            "Loan",
            "Investment",
            "Savings",
            "Interest",
            "Dividends",
            "Rental",
            "Sale",
            "Bonus",
            "Award",
            "Prize",
            "Grant",
            "Scholarship",
            "Allowance"
        });
    }

    public Task<List<string>> GetOutcomingCategoriesAsync() {
        return Task.FromResult(new List<string> {
            "Food",
            "Transport",
            "Other"
        });
    }
}