using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.AsyncDataServices;
using TelegramService.Contracts;
using TelegramService.SyncDataServices.Http;

namespace TelegramService.WalletStates;

public class WalletContext : IWalletContext
{
    private IWalletState _currentState;
    private readonly ITelegramBotClient _bot;
    private readonly ILoggerManager _logger;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IWalletDataClient _walletDataClient;

    public WalletContext(ITelegramBotClient bot, ILoggerManager logger, IWalletDataClient walletDataClient, IMessageBusClient messageBusClient)
    {
        _bot = bot;
        _logger = logger;
        _messageBusClient = messageBusClient;
        _currentState = new StartState();
        _walletDataClient = walletDataClient;
        _currentState.Init(this, bot, logger, messageBusClient, walletDataClient);

        _walletDataClient.TestInboundConnection();
    }

    public async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"WalletContext: Received message: {message.Text}, current state: {_currentState.GetType().Name}");
        await _currentState.HandleRequest(message, cancellationToken);
    }

    public async Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"WalletContext: Received callback query: {callbackQuery.Data}, current state: {_currentState.GetType().Name}");
        await _currentState.HandleCallbackQuery(callbackQuery, cancellationToken);
    }

    public void SetState(IWalletState newState)
    {
        _logger.LogInfo($"WalletContext: Changed state from {_currentState.GetType().Name} to {newState.GetType().Name}");
        _currentState = newState;
        _currentState.Init(this, _bot, _logger, _messageBusClient, _walletDataClient);
    }

    public IWalletState GetCurrentState()
    {
        return _currentState;
    }
}