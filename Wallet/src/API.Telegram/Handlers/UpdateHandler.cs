using API.Telegram.Contracts;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace API.Telegram.Handlers;

public class UpdateHandler(ILoggerManager logger, IWalletContext walletContext) : IUpdateHandler
{
    [Authorize]
    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        logger.LogInfo($"Received update type: {update.Type.ToString()}");
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceivedAsync(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceivedAsync(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update)
        };

        return handler;
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError($"An error occurred in {source}: {exception.Message}");
        logger.LogError($"Exception details: {exception}");
        return Task.CompletedTask;
    }

    private Task BotOnMessageReceivedAsync(Message message, CancellationToken cancellationToken)
    {
        logger.LogInfo($"Receive message type: {message.Text}");
        return walletContext.HandleRequestAsync(message, cancellationToken);
    }

    private Task BotOnCallbackQueryReceivedAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        logger.LogInfo($"Receive callback query type: {callbackQuery.Data}");
        return walletContext.HandleCallbackQueryAsync(callbackQuery, cancellationToken);
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
        {
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}
