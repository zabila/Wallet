using Telegram.Bot;
using Telegram.Bot.Types;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.SyncDataServices.Http;

namespace Wallet.Services.Telegram.Contracts;

public interface IWalletState {
    void Init(IWalletContext context, ITelegramBotClient client, ILoggerManager logger, IMessageBusClient messageBusClient, IWalletDataClient walletDataClient);
    Task HandleRequest(Message message, CancellationToken cancellationToken);
    Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}