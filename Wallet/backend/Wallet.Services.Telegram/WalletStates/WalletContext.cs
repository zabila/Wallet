using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Extensions;
using Wallet.Services.Telegram.Resources;
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
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(session.Localization.EnsureExists());

        BotTrigger? trigger = ParseLocalizedBotTrigger(text.EnsureExists());

        var machine = session.CurrentStateMachine.EnsureExists();
        if (!trigger.HasValue) {
            (bool isReprocessable, BotTrigger reprocessableTrigger) = IsStateReprocessable(machine.State);
            if (isReprocessable) {
                await machine.FireAsync(reprocessableTrigger, message);
                return;
            }

            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger.Value)) {
            await machine.FireAsync(trigger.Value, message);
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

        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(session.Localization.EnsureExists());

        var machine = session.CurrentStateMachine.EnsureExists();
        var triggerSrt = text.Split(":").First();
        BotTrigger? trigger = ParseLocalizedBotTrigger(triggerSrt);
        if (!trigger.HasValue) {
            (bool isReprocessable, BotTrigger reprocessableTrigger) = IsStateReprocessable(machine.State);
            if (isReprocessable) {
                await machine.FireAsync(reprocessableTrigger, message);
                return;
            }

            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger.Value)) {
            await machine.FireAsync(trigger.Value, data);
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

    private static BotTrigger? ParseLocalizedBotTrigger(string localizedText) {
        foreach (BotTrigger trigger in Enum.GetValues(typeof(BotTrigger))) {
            string description = trigger.GetLocalizedDescription();
            if (string.Equals(description, localizedText, StringComparison.OrdinalIgnoreCase)) {
                return trigger;
            }
        }

        return null;
    }
}