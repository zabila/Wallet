namespace Wallet.Services.Telegram.Contracts;

/// <summary>
/// Represents a service capable of receiving and processing asynchronous updates or events from an external source.
/// </summary>
public interface IReceiverService {
    /// <summary>
    /// Receives updates or events asynchronously from an external source, processes them, and logs the status of the operation.
    /// </summary>
    /// <param name="stoppingToken">A token that propagates notification that the process should halt.</param>
    /// <returns>A task representing the asynchronous operation of receiving and handling updates.</returns>
    Task ReceiveAsync(CancellationToken stoppingToken);
}