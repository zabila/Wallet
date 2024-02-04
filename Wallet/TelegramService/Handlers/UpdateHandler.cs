using AutoMapper;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Microsoft.AspNetCore.Authorization;
using TelegramService.Contracts;
using TelegramService.WalletStates;

namespace TelegramService.Handlers;

public class UpdateHandler(ILoggerManager logger, IMapper mapper, IWalletContext walletContext) : IUpdateHandler
{
    private readonly IMapper _mapper = mapper;

    [Authorize]
    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        logger.LogInfo($"Received update type: {update.Type.ToString()}");
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update)
        };
        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText)
            return;

        logger.LogInfo($"Receive message type: {message.Text}");
        await walletContext.HandleRequest(message, cancellationToken);
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var message = callbackQuery.Message;
        if (message?.Text is not { } messageText)
            return;

        logger.LogInfo($"Receive callback query type: {callbackQuery.Data}");
        await walletContext.HandleCallbackQuery(callbackQuery, cancellationToken);
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInfo($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogInfo("HandleError: " + errorMessage);

        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}