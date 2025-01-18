using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Abstract;

public abstract class PollingServiceBase<TReceiverService> : BackgroundService
    where TReceiverService : IReceiverService {
    private readonly IServiceProvider _serviceScopeFactory;
    private readonly ILoggerManager _logger;
    private const int PollingDelayInSeconds = 3;

    internal PollingServiceBase(IServiceProvider serviceScopeFactory, ILoggerManager logger) {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInfo("Polling service is starting.");
        return PerformPollingAsync(stoppingToken);
    }

    private async Task PerformPollingAsync(CancellationToken stoppingToken) {
        _logger.LogInfo("Polling service is working.");
        while (!stoppingToken.IsCancellationRequested) {
            await PerformReceiveCycleAsync(stoppingToken);
        }
    }

    private async Task PerformReceiveCycleAsync(CancellationToken stoppingToken) {
        try {
            using var scope = _serviceScopeFactory.CreateScope();
            var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();
            await receiver.ReceiveAsync(stoppingToken);
            _logger.LogInfo($"Polling service for {typeof(TReceiverService).Name} completed successfully.");
        } catch (Exception ex) {
            _logger.LogError($"Error in {typeof(TReceiverService).Name} polling service: {ex.Message}");
            await Task.Delay(TimeSpan.FromSeconds(PollingDelayInSeconds), stoppingToken);
        }
    }
}