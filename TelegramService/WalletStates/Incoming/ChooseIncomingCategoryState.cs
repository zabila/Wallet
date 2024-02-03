using Telegram.Bot.Types;
using TelegramService.Abstract;

namespace TelegramService.WalletStates.Incoming;

public class ChooseIncomingCategoryState : WalletStateBase
{
    public override Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}