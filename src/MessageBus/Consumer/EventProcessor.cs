using System.Text.Json;
using Application.Transactions.Create;
using MediatR;
using MessageBus.Events;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions;
using SharedKernel.DTO.Transactions;
using SharedKernel.Extensions;

namespace MessageBus.Consumer;

public class EventProcessor : IEventProcessor, IDisposable
{
    private readonly ILoggerManager _logger;
    private readonly IServiceScope _scope;
    private readonly ISender _sender;


    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
        _sender = _scope.ServiceProvider.GetRequiredService<ISender>().EnsureExists();
        _logger = _scope.ServiceProvider.GetRequiredService<ILoggerManager>().EnsureExists();
    }

    public void Dispose()
    {
        _logger.LogInfo("EventProcessor Disposed");
        _scope.Dispose();
    }

    public async Task ProcessEventAsync(string message, CancellationToken cancellationToken)
    {
        var eventType = DetermineEventType(message);
        switch (eventType)
        {
            case EventType.CreateTransactionEvent:
                await ProcessCreateTransactionEventAsync(message, cancellationToken);
                break;
            case EventType.Undetermined:
                _logger.LogError("Not processing event");
                break;
            default:
                _logger.LogError("Could not determine event type");
                break;
        }
    }

    private async Task ProcessCreateTransactionEventAsync(string message, CancellationToken cancellationToken)
    {
        _logger.LogInfo("Processing CreateTransactionEvent event");
        try
        {
            var createTransactionEvent = JsonSerializer.Deserialize<CreateTransactionEvent>(message).EnsureExists();
            var createTransactionCommand = new CreateTransactionCommand
            {
                UserId = createTransactionEvent.UserId,
                Amount = createTransactionEvent.Amount,
                Category = createTransactionEvent.Category,
                Type = createTransactionEvent.Type,
                Location = createTransactionEvent.Location ?? new Location(),
                Attachment = createTransactionEvent.Attachment
            };

            var transaction = await _sender.Send(createTransactionCommand, cancellationToken);
            _logger.LogInfo($"Transaction created: {transaction}");
        }
        catch (Exception exception)
        {
            _logger.LogError($"Something went wrong: {exception.Message}");
        }
    }

    private EventType DetermineEventType(string message)
    {
        _logger.LogInfo($"Determining event type {message}");
        var eventType = JsonSerializer.Deserialize<GenericEvent>(message).EnsureExists();
        switch (eventType.Event)
        {
            case "CreateTransactionEvent":
                _logger.LogInfo("CreateTransactionEvent event detected");
                return EventType.CreateTransactionEvent;
            default:
                _logger.LogError("Could not determine event type");
                return EventType.Undetermined;
        }
    }
}
