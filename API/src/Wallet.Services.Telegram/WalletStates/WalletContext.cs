using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.SyncDataServices.Http;

namespace Wallet.Services.Telegram.WalletStates;

public class WalletContext : IWalletContext {
    private readonly ITelegramBotClient _bot;
    private readonly ILoggerManager _logger;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IWalletDataClient _walletDataClient;
    private readonly ISessionManager _sessionManger;


    public WalletContext(ITelegramBotClient bot, ILoggerManager logger, IWalletDataClient walletDataClient, IMessageBusClient messageBusClient, ISessionManager sessionManger) {
        _bot = bot;
        _logger = logger;
        _messageBusClient = messageBusClient;
        _sessionManger = sessionManger;
        _walletDataClient = walletDataClient;

        _walletDataClient.TestInboundConnection();
    }

    public async Task HandleRequestAsync(Message message, CancellationToken cancellationToken) {
        var chatId = message.Chat.Id;
        var text = message.Text;

        var session = await _sessionManger.GetOrCreateSessionAsync(chatId);
        session.LastInteractionTime = DateTime.UtcNow;

        if (!Enum.TryParse<BotTrigger>(text, ignoreCase: true, out var trigger)) {
            await _bot.SendTextMessageAsync(chatId, "I don't understand you.", cancellationToken: cancellationToken);
            return;
        }

        var machine = session.CurrentStateMachine;
        if (machine.CanFire(trigger)) {
            await machine.FireAsync(trigger);
        } else {
            await _bot.SendTextMessageAsync(chatId, "Oops! Something went wrong.", cancellationToken: cancellationToken);
        }
    }

    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken) {
    }
}