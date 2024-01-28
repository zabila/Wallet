using Telegram.Bot.Types;
using TelegramService.Abstract;
using TelegramService.Dtos;
using TelegramService.Services;

namespace TelegramService.WalletStates.Incoming;

public class IncomingState : WalletStateBase
{
    public override Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        var messageTransaction = message.Text;
        Logger.LogInfo($"message.Text: {message.Text}");

        var transaction = new TransactionPublishedDto()
        {
            Amount = 20,
            Category = "Product",
            Type = "Incoming"
        };
        transaction.Event = "Wallet_Published";
        MessageBusClient.PublishNewTransaction(transaction);
        return Task.CompletedTask;
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}