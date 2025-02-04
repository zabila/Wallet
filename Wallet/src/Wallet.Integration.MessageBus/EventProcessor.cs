using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Application.Finance.Account.Queries;
using Wallet.Application.Finance.Transaction.Commands;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Enums;
using Wallet.Shared.DataTransferObjects;
using Wallet.Shared.Extensions;

namespace Wallet.Integration.MessageBus;

public class EventProcessor : IEventProcessor, IDisposable
{
    private readonly ISender _sender;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IServiceScope _scope;


    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
        _sender = _scope.ServiceProvider.GetRequiredService<ISender>().EnsureExists();
        _logger = _scope.ServiceProvider.GetRequiredService<ILoggerManager>().EnsureExists();
        _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>().EnsureExists();
    }

    public async Task ProcessEventAsync(string message)
    {
        var eventType = DetermineEventType(message);
        switch (eventType)
        {
            case EventType.TransactionTelegramPublished:
                await ProcessTransactionTelegramPublishedAsync(message);
                break;
            case EventType.Undetermined:
                _logger.LogError("Not processing event");
                break;
            default:
                _logger.LogError("Could not determine event type");
                break;
        }
    }

    private async Task ProcessTransactionTelegramPublishedAsync(string message)
    {
        _logger.LogInfo("Processing TransactionTelegramPublished event");
        var transactionPublishedDto = JsonSerializer.Deserialize<TransactionPublishedDto>(message).EnsureExists();
        _logger.LogInfo($"Processing TransactionTelegramPublished event: {transactionPublishedDto}");
        try
        {
            var transactionCreateDto = _mapper.Map<TransactionCreateDto>(transactionPublishedDto).EnsureExists();

            var account = await _sender.Send(new GetAccountByTelegramUserIdQuery(transactionPublishedDto.TelegramUserId));
            var transaction = await _sender.Send(new CreateTransactionCommand(account.Id, transactionCreateDto));
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
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message).EnsureExists();
        switch (eventType.Event)
        {
            case "TransactionTelegramPublished":
                _logger.LogInfo("TransactionTelegramPublished event detected");
                return EventType.TransactionTelegramPublished;
            default:
                _logger.LogError("Could not determine event type");
                return EventType.Undetermined;
        }
    }

    public void Dispose()
    {
        _logger.LogInfo("EventProcessor Disposed");
        _scope.Dispose();
    }
}