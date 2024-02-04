using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Abstract;

public abstract class PollingServiceBase<TReceiverService> : BackgroundService where TReceiverService : IReceiverService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggerManager _logger;

    internal PollingServiceBase(IServiceProvider serviceProvider, ILoggerManager logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInfo("Polling service is starting.");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInfo("Polling service is working.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();
                await receiver.ReceiveAsync(stoppingToken);
                _logger.LogInfo("Polling service is done.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in polling service: " + ex.Message);
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }
}