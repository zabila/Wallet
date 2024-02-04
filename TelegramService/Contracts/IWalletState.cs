using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.AsyncDataServices;
using TelegramService.SyncDataServices.Http;

namespace TelegramService.Contracts;

public interface IWalletState
{
    void Init(IWalletContext context, ITelegramBotClient client, ILoggerManager logger, IMessageBusClient messageBusClient, IWalletDataClient walletDataClient);
    Task HandleRequest(Message message, CancellationToken cancellationToken);
    Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}