using Telegram.Bot.Types;
using TelegramService.Abstract;

namespace TelegramService.WalletStates.Outcoming;

public class OutcomingState : WalletStateBase
{
    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        Context!.SetState(new ChooseOutcomingCategoryState());
        await Context!.HandleRequest(message, cancellationToken);
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}