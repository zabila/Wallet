using System.Text;
using Newtonsoft.Json;
using TelegramService.Contracts;

namespace TelegramService.SyncDataServices.Http;

public class HttpWalletDataClient(HttpClient httpClient, IConfiguration configuration, ILoggerManager logger) : IWalletDataClient
{
    public async Task TestInboundConnection()
    {
        var httpContent = new StringContent("",
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.PostAsync($"{configuration["WalletService"]}", httpContent);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInfo("Sync POST to WalletService was OK!");
        }
        else
        {
            logger.LogError("Sync POST to WalletService was NOT OK!");
        }
    }

    public Task<List<string>> GetIncomingCategoriesAsync()
    {
        return Task.FromResult(new List<string>
        {
            "Salary",
            "Gift",
            "Other"
        });
    }

    public Task<List<string>> GetOutcomingCategoriesAsync()
    {
        return Task.FromResult(new List<string>
        {
            "Food",
            "Transport",
            "Other"
        });
    }
}