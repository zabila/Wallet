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
            text: "Введіть суму витрат, а якщо ви хочете добавити суму з новою категорію напишіть {категорія} {сума}, категорія за замовчуванням буде Загальні",
            cancellationToken: cancellationToken);
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}