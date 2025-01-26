using Telegram.Bot.Types;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates;

public class WalletContext : IWalletContext {
    private readonly ISessionManager _sessionManger;
    private readonly IBotStateMachineFactory _botStateMachineFactory;

    public WalletContext(IWalletDataClient walletDataClient, ISessionManager sessionManger, IBotStateMachineFactory botStateMachineFactory) {
        _sessionManger = sessionManger;
        _botStateMachineFactory = botStateMachineFactory;

        walletDataClient.TestInboundConnection();
    }

    public async Task HandleRequestAsync(Message message, CancellationToken cancellationToken) {
        var chatId = message.Chat.Id;
        var text = message.Text;

        var session = await _sessionManger.GetOrCreateSessionAsync(chatId);
        session.LastInteractionTime = DateTime.UtcNow;

        var machine = session.CurrentStateMachine.EnsureExists();
        if (!Enum.TryParse<BotTrigger>(text, ignoreCase: true, out var trigger) || !Enum.IsDefined(typeof(BotTrigger), trigger)) {
            (bool isReprocessable, BotTrigger reprocessableTrigger) = IsStateReprocessable(machine.State);
            if (isReprocessable) {
                await machine.FireAsync(reprocessableTrigger, message);
                return;
            }

            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger)) {
            await machine.FireAsync(trigger);
        } else {
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
        if (!Enum.TryParse<BotTrigger>(triggerSrt, ignoreCase: true, out var trigger) || !Enum.IsDefined(typeof(BotTrigger), trigger)) {
            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger)) {
            await machine.FireAsync(trigger, data);
        } else {
            await machine.FireAsync(BotTrigger.Error);
        }
    }

    private Tuple<bool, BotTrigger> IsStateReprocessable(BotState state) {
        _botStateMachineFactory.StateDefinition.TryGetValue(state, out var definition);
        return definition?.ShouldBeRecalled ?? Tuple.Create(false, BotTrigger.Error);
    }
}