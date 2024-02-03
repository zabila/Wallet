using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.Abstract;

namespace TelegramService.WalletStates.Outcoming;

public class GetAmountState : WalletStateBase
{
    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        Context!.SetState(new SaveAmountState());

        await BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Enter the amount of expenses, and if you want to add an amount with a new category, write {category} {amount}, the default category will be General",
            cancellationToken: cancellationToken);
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}