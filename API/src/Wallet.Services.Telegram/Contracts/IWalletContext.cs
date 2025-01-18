using Telegram.Bot.Types;

namespace Wallet.Services.Telegram.Contracts;

public interface IWalletContext {
    Task HandleRequestAsync(Message message, CancellationToken cancellationToken);

    Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}