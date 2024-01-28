using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.AsyncDataServices;
using TelegramService.Contracts;

namespace TelegramService.Abstract;

public abstract class WalletStateBase : IWalletState
{
    protected IWalletContext Context = null!;
    protected ITelegramBotClient BotClient = null!;
    protected ILoggerManager Logger = null!;
    protected IMessageBusClient MessageBusClient = null!;

    public void Init(IWalletContext context, ITelegramBotClient client, ILoggerManager logger, IMessageBusClient messageBusClient)
    {
        Context = context;
        BotClient = client;
        Logger = logger;
        MessageBusClient = messageBusClient;
    }

    public abstract Task HandleRequest(Message message, CancellationToken cancellationToken);
    public abstract Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}