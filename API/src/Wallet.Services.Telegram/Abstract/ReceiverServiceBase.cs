using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Abstract;

public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService where TUpdateHandler : IUpdateHandler {
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILoggerManager _logger;

    private readonly ReceiverOptions _receiverOptions = new ReceiverOptions {
        AllowedUpdates = []
    };

    internal ReceiverServiceBase(ITelegramBotClient botClient, TUpdateHandler updateHandler, ILoggerManager logger) {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken stoppingToken) {
        var me = await _botClient.GetMe(stoppingToken);
        _logger.LogInfo($"Start listening for @{me.Username}");

        await _botClient.ReceiveAsync(updateHandler: _updateHandler, receiverOptions: _receiverOptions, cancellationToken: stoppingToken);
    }
}