namespace MessageBus.Consumer;

public interface IEventProcessor
{
    Task ProcessEventAsync(string message, CancellationToken cancellationToken);
}
