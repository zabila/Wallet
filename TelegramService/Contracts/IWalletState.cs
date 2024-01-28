using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.AsyncDataServices;

namespace TelegramService.Contracts;

public interface IWalletState
{
    void Init(IWalletContext context, ITelegramBotClient client, ILoggerManager logger, IMessageBusClient messageBusClient);
    Task HandleRequest(Message message, CancellationToken cancellationToken);
    Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}