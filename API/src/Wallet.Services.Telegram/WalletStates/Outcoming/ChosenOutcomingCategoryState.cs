using Telegram.Bot.Types;
using Wallet.Services.Telegram.Abstract;
using Wallet.Services.Telegram.Dtos;

namespace Wallet.Services.Telegram.WalletStates.Outcoming;

public class ChosenOutcomingCategoryState(string? category) : WalletStateBase {
    public override Task HandleRequest(Message message, CancellationToken cancellationToken) {
        var amount = 0;
        var messageText = message.Text;
        if (category == null && messageText!.Split(" ").Length > 1) {
            var split = messageText.Split(" ");
            category = split[0];
            amount = int.Parse(split[1]);
        } else {
            amount = int.Parse(messageText!);
        }

        var transaction = new TransactionPublishedDto {
            Id = Guid.NewGuid(),
            TelegramUserId = (int)message.From!.Id,
            Date = message.Date,
            Category = category,
            Type = "Outcoming",
            Amount = amount,
            Description = "Transaction from telegram bot",
            Tags = "Telegram",
            Currency = "UAH",
            Location = message.Location != null ? message.Location.ToString() : string.Empty
        };

        MessageBusClient.PublishNewTransaction(transaction);

        Context!.SetState(new StartState());

        return Task.CompletedTask;
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}