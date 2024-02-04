namespace Wallet.Domain.Contracts;

public interface IEventProcessor
{
    Task ProcessEvent(string message);
}