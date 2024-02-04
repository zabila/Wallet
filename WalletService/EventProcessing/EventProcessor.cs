using System.Text.Json;
using Application.Account.Queries;
using Application.Transaction.Commands;
using AutoMapper;
using Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.DataTransferObjects;

namespace EventProcessing;

public class EventProcessor : IEventProcessor
{
    private ISender? _sender;
    private ILoggerManager? _logger;
    private IMapper? _mapper;
    private IRepositoryManager _repositoryManager;


    public EventProcessor(IServiceScopeFactory scopeFactory)
    {
        var scoped = scopeFactory.CreateScope();
        _sender = scoped.ServiceProvider.GetRequiredService<ISender>();
        _logger = scoped.ServiceProvider.GetRequiredService<ILoggerManager>();
        _mapper = scoped.ServiceProvider.GetRequiredService<IMapper>();
        _repositoryManager = scoped.ServiceProvider.GetRequiredService<IRepositoryManager>();
    }

    public async Task ProcessEvent(string message)
    {
        var eventType = DetermineEventType(message);
        switch (eventType)
        {
            case EventType.TransactionTelegramPublished:
                await ProcessTransactionTelegramPublished(message);
                break;
            case EventType.Undetermined:
                _logger.LogError("Could not determine event type");
                break;
            default:
                _logger.LogError("Could not determine event type");
                break;
        }
    }

    private async Task ProcessTransactionTelegramPublished(string message)
    {
        var transactionPublishedDto = JsonSerializer.Deserialize<TransactionPublishedDto>(message);
        try
        {
            var transactionCreateDto = _mapper!.Map<TransactionCreateDto>(transactionPublishedDto);

            var account = await _sender!.Send(new GetAccountByTelegramUserIdQuery(transactionPublishedDto!.TelegramUserId));
            var transaction = await _sender!.Send(new CreateTransactionCommand(account.Id, transactionCreateDto));
            _logger!.LogInfo($"Transaction created: {transaction}");
        }
        catch (Exception exception)
        {
            _logger!.LogError($"Something went wrong: {exception.Message}");
        }
    }

    private EventType DetermineEventType(string message)
    {
        _logger!.LogInfo($"Determining event type {message}");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);
        switch (eventType!.Event)
        {
            case "TransactionTelegramPublished":
                _logger.LogInfo("TransactionTelegramPublished event detected");
                return EventType.TransactionTelegramPublished;
            default:
                _logger.LogError("Could not determine event type");
                return EventType.Undetermined;
        }
    }
}