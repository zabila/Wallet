namespace Wallet.Services.Telegram.Contracts;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}