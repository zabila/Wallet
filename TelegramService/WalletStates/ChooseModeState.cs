using Telegram.Bot.Types;
using TelegramService.Abstract;
using TelegramService.WalletStates.Incoming;
using TelegramService.WalletStates.Outcoming;

namespace TelegramService.WalletStates;

public class ChooseModeState : WalletStateBase
{
    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        var command = message.Text;
        if (command == "Expenses")
        {
            Context!.SetState(new OutcomingState());
            await Context!.HandleRequest(message, cancellationToken);
        }
        else if (command == "Income")
        {
            Context!.SetState(new IncomingState());
            await Context!.HandleRequest(message, cancellationToken);
        }
        else
        {
            Context!.SetState(new StartState());
            await Context!.HandleRequest(message, cancellationToken);
        }
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}