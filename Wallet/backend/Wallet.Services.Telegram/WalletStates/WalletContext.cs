using Telegram.Bot;
using Telegram.Bot.Types;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates;

public class WalletContext(ILoggerManager logger, ITelegramBotClient botClient, ISessionManager sessionManger, IBotStateMachineFactory botStateMachineFactory, IWalletFinanceAccountClient financeAccountClient) : IWalletContext {
    public async Task HandleRequestAsync(Message message, CancellationToken cancellationToken) {
        var chatId = (int)message.EnsureExists().From.EnsureExists().Id;
        if (!await IsHasPermissionAsync(chatId)) {
            return;
        }

        var text = message.Text;

        if (text == null && message.Location != null) {
            text = "ShareLocation";
        }

        var session = await sessionManger.GetOrCreateSessionAsync(chatId);
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
            await machine.FireAsync(trigger, message);
        } else {
            await machine.FireAsync(BotTrigger.Error);
        }
    }

    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken) {
        var message = callbackQuery.EnsureExists().Message.EnsureExists();
        var chatId = message.Chat.Id;
        if (!await IsHasPermissionAsync((int)chatId)) {
            return;
        }

        var data = callbackQuery.Data.EnsureExists();
        var text = message.Text.EnsureExists();

        var session = await sessionManger.GetOrCreateSessionAsync(chatId);
        session.LastInteractionTime = DateTime.UtcNow;

        var machine = session.CurrentStateMachine.EnsureExists();
        var triggerSrt = text.Split(":").First();
        if (!Enum.TryParse<BotTrigger>(triggerSrt, ignoreCase: true, out var trigger) || !Enum.IsDefined(typeof(BotTrigger), trigger)) {
            (bool isReprocessable, BotTrigger reprocessableTrigger) = IsStateReprocessable(machine.State);
            if (isReprocessable) {
                await machine.FireAsync(reprocessableTrigger, message);
                return;
            }

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
        botStateMachineFactory.StateDefinition.TryGetValue(state, out var definition);
        return definition?.ShouldBeRecalled ?? Tuple.Create(false, BotTrigger.Error);
    }

    private async Task<bool> IsHasPermissionAsync(int chatId) {
        var accountId = await financeAccountClient.GetAccountIdByTelegramUserIdAsync(chatId);
        bool isAccountFound = accountId != null && accountId.Id != Guid.Empty;
        if (!isAccountFound) {
            logger.LogError($"Account not found or don't have permission to access the account. ChatId: {chatId}");
            await botClient.SendMessage(chatId, "You don't have permission to access the account. Please contact the administrator.");
        } else {
            logger.LogInfo($"Account found. ChatId: {chatId}, AccountId: {accountId.EnsureExists().Id}");
        }

        return isAccountFound;
    }
}