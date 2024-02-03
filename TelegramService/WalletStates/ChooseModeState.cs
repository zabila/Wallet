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
        switch (command)
        {
            case "Expenses":
                Context!.SetState(new OutcomingState());
                await Context!.HandleRequest(message, cancellationToken);
                break;
            case "Income":
                Context!.SetState(new IncomingState());
                await Context!.HandleRequest(message, cancellationToken);
                break;
            default:
                Context!.SetState(new StartState());
                await Context!.HandleRequest(message, cancellationToken);
                break;
        }
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}