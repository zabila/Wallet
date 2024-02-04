using Telegram.Bot.Types;

namespace Wallet.Services.Telegram.Contracts;

public interface IWalletContext
{
    Task HandleRequest(Message message, CancellationToken cancellationToken);

    Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);

    void SetState(IWalletState newState);

    IWalletState GetCurrentState();
}