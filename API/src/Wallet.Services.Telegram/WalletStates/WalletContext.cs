using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates;

public class WalletContext : IWalletContext {
    private readonly ITelegramBotClient _bot;
    private readonly ILoggerManager _logger;
    private readonly ISessionManager _sessionManger;


    public WalletContext(ITelegramBotClient bot, ILoggerManager logger, IWalletDataClient walletDataClient, IMessageBusClient messageBusClient, ISessionManager sessionManger) {
        _bot = bot;
        _logger = logger;
        _sessionManger = sessionManger;

        walletDataClient.TestInboundConnection();
    }

    public async Task HandleRequestAsync(Message message, CancellationToken cancellationToken) {
        var chatId = message.Chat.Id;
        var text = message.Text;

        var session = await _sessionManger.GetOrCreateSessionAsync(chatId);
        session.LastInteractionTime = DateTime.UtcNow;

        var machine = session.CurrentStateMachine.EnsureExists();
        if (!Enum.TryParse<BotTrigger>(text, ignoreCase: true, out var trigger)) {
            await _bot.SendMessage(chatId, "I don't understand you.", cancellationToken: cancellationToken);
            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger)) {
            await machine.FireAsync(trigger);
        } else {
            await _bot.SendMessage(chatId, "Oops! Something went wrong.", cancellationToken: cancellationToken);
            await machine.FireAsync(BotTrigger.Error);
        }
    }

    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken) {
        var message = callbackQuery.EnsureExists().Message.EnsureExists();
        var data = callbackQuery.Data.EnsureExists();
        var chatId = message.Chat.Id;
        var text = message.Text.EnsureExists();

        var session = await _sessionManger.GetOrCreateSessionAsync(chatId);
        session.LastInteractionTime = DateTime.UtcNow;

        var machine = session.CurrentStateMachine.EnsureExists();
        var triggerSrt = text.Split(":").First();
        if (!Enum.TryParse<BotTrigger>(triggerSrt, ignoreCase: true, out var trigger)) {
            await _bot.SendMessage(chatId, "I don't understand this bottom", cancellationToken: cancellationToken);
            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger)) {
            await machine.FireAsync(trigger, data);
        } else {
            await _bot.SendMessage(chatId, "Oops! Something went wrong on pressed bottom.", cancellationToken: cancellationToken);
            await machine.FireAsync(BotTrigger.Error);
        }
    }
}