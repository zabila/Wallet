using Telegram.Bot.Types;

namespace API.Telegram.Contracts;

public interface IWalletContext
{
    Task HandleRequestAsync(Message message, CancellationToken cancellationToken);

    Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}
