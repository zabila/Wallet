using MessageBus.Events;

namespace MessageBus.Publisher;

public interface IMessageBusClient
{
    Task PublishCreateTransactionEventAsync(CreateTransactionEvent createTransactionEvent);
}
