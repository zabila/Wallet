using API.Telegram.Contracts;
using SharedKernel.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace API.Telegram.Abstract;

public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService where TUpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILoggerManager _logger;

    private readonly ReceiverOptions _receiverOptions = new()
    {
        AllowedUpdates = []
    };

    private readonly IUpdateHandler _updateHandler;

    protected ReceiverServiceBase(ITelegramBotClient botClient, TUpdateHandler updateHandler, ILoggerManager logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        var me = await _botClient.GetMe(stoppingToken);
        _logger.LogInfo($"Start listening for @{me.Username}");

        await _botClient.ReceiveAsync(_updateHandler, _receiverOptions, stoppingToken);
    }
}
