namespace Wallet.Domain.Contracts;

public interface IEventProcessor {
    Task ProcessEventAsync(string message);
}