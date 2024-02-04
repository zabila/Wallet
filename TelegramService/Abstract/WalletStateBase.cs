using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.AsyncDataServices;
using TelegramService.Contracts;
using TelegramService.SyncDataServices.Http;

namespace TelegramService.Abstract;

public abstract class WalletStateBase : IWalletState
{
    protected IWalletContext Context = null!;
    protected ITelegramBotClient BotClient = null!;
    protected ILoggerManager Logger = null!;
    protected IMessageBusClient MessageBusClient = null!;
    protected IWalletDataClient WalletDataClient = null!;

    public void Init(IWalletContext context, ITelegramBotClient client, ILoggerManager logger, IMessageBusClient messageBusClient, IWalletDataClient walletDataClient)
    {
        Context = context;
        BotClient = client;
        Logger = logger;
        MessageBusClient = messageBusClient;
        WalletDataClient = walletDataClient;
    }

    public abstract Task HandleRequest(Message message, CancellationToken cancellationToken);
    public abstract Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}