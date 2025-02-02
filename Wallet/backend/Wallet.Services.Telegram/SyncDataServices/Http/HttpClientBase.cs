using Flurl.Http;
using Wallet.Services.Telegram.Contracts;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.SyncDataServices.Http;

public abstract class HttpClientBase(ILoggerManager logger, IFlurlClient httpClient) {
    private IFlurlRequest GetHttp(string path) {
        return httpClient.Request(path)
            .WithOAuthBearerToken(Environment.GetEnvironmentVariable("WALLET_API_TOKEN").EnsureExists())
            .WithHeader("Accept", "application/json")
            .WithHeader("Content-Type", "application/json");
    }

    protected async Task<T> PostAsync<T>(string endpoint, HttpContent? content = null) {
        try {
            var http = GetHttp(endpoint);
            T result = await http.PostAsync(content).ReceiveJson<T>();
            return result;
        } catch (FlurlHttpException ex) {
            logger.LogError($"Error returned from {ex.Call.Request.Url}: Error: {ex.Message}");
        } catch (Exception ex) {
            logger.LogError($"Error while make request to {endpoint}: {ex.Message}");
        }

        throw new InvalidOperationException();
    }

    protected async Task<T> GetAsync<T>(string endpoint) {
        try {
            var http = GetHttp(endpoint);
            T result = await http.GetAsync().ReceiveJson<T>();
            return result;
        } catch (FlurlHttpException ex) {
            logger.LogError($"Error returned from {ex.Call.Request.Url}: Error: {ex.Message}");
        } catch (Exception ex) {
            logger.LogError($"Error while make request to {endpoint}: {ex.Message}");
        }

        throw new InvalidOperationException();
    }
}