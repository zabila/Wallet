namespace TelegramService.Contracts;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}