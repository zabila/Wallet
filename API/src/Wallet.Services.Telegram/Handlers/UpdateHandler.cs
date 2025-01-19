using Microsoft.AspNetCore.Authorization;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Handlers;

/// <summary>
/// The UpdateHandler class is responsible for handling and processing updates received from the Telegram Bot API.
/// It implements the IUpdateHandler interface and provides functionality to handle various types of updates,
/// including messages and callback queries, as well as managing polling errors.
/// </summary>
public class UpdateHandler(ILoggerManager logger, IWalletContext walletContext) : IUpdateHandler {
    /// <summary>
    /// Handles incoming Telegram updates and processes them based on their type. Determines if the update
    /// contains a message, callback query, or is of an unknown type, and executes the appropriate handler
    /// method for each case.
    /// </summary>
    /// <param name="update">The received update object from Telegram.</param>
    /// <param name="cancellationToken">A token to monitor for task cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [Authorize]
    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken) {
        logger.LogInfo($"Received update type: {update.Type.ToString()}");
        var handler = update switch {
            { Message: { } message } => BotOnMessageReceivedAsync(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceivedAsync(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update)
        };

        await handler;
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken) {
        logger.LogError($"An error occurred in {source}: {exception.Message}");
        logger.LogError($"Exception details: {exception}");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles incoming messages received from the Telegram bot.
    /// </summary>
    /// <param name="message">The message object containing the content of the received message.</param>
    /// <param name="cancellationToken">Token to observe for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task BotOnMessageReceivedAsync(Message message, CancellationToken cancellationToken) {
        if (message.Text is not { } messageText)
            return;

        logger.LogInfo($"Receive message type: {message.Text}");
        await walletContext.HandleRequestAsync(message, cancellationToken);
    }

    /// <summary>
    /// Handles the callback query received from a Telegram bot update.
    /// </summary>
    /// <param name="callbackQuery">The callback query object containing details of the interaction.</param>
    /// <param name="cancellationToken">A token to notify about cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task BotOnCallbackQueryReceivedAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken) {
        var message = callbackQuery.Message;
        if (message?.Text is not { } messageText)
            return;

        logger.LogInfo($"Receive callback query type: {callbackQuery.Data}");
        await walletContext.HandleCallbackQueryAsync(callbackQuery, cancellationToken);
    }

    /// <summary>
    /// Handles updates with unknown types in the application. Logs the update type
    /// information and executes a completed task without performing any operations.
    /// </summary>
    /// <param name="update">The update object received from Telegram Bot API.</param>
    /// <returns>A completed Task representing the asynchronous operation with no result.</returns>
    private Task UnknownUpdateHandlerAsync(Update update) {
        logger.LogInfo($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles errors that occur during the polling process for updates in the Telegram Bot.
    /// </summary>
    /// <param name="botClient">The Telegram Bot client instance responsible for handling requests.</param>
    /// <param name="exception">The exception that occurred during the polling process.</param>
    /// <param name="cancellationToken">The token used to propagate notification that the operation should be canceled.</param>
    /// <returns>A Task that represents the asynchronous operation for handling the error.</returns>
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
        var errorMessage = exception switch {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogInfo("HandleError: " + errorMessage);

        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}