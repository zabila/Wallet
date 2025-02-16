using System.Globalization;
using API.Telegram.Contracts;
using API.Telegram.Extensions;
using API.Telegram.Models;
using SharedKernel.Abstractions;
using SharedKernel.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace API.Telegram.WalletStates;

public class WalletContext(ILoggerManager logger, ITelegramBotClient botClient, ISessionManager sessionManger, IBotStateMachineFactory botStateMachineFactory, IWalletIdentityClient walletIdentityClient) : IWalletContext
{
    public async Task HandleRequestAsync(Message message, CancellationToken cancellationToken)
    {
        var userId = (int)message.EnsureExists().From.EnsureExists().Id;
        var text = message.Text;
        if (text == null && message.Location != null)
        {
            text = "ShareLocation";
        }

        var session = await sessionManger.GetOrCreateSessionAsync(userId);
        var logedUser = session.LoggenUser.EnsureExists();
        if (!await IsHasPermissionAsync(session))
        {
            return;
        }

        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(logedUser.Localization.EnsureExists());

        var trigger = ParseLocalizedBotTrigger(text.EnsureExists());

        var machine = session.CurrentStateMachine.EnsureExists();
        if (!trigger.HasValue)
        {
            (var isReprocessable, var reprocessableTrigger) = IsStateReprocessable(machine.State);
            if (isReprocessable)
            {
                await machine.FireAsync(reprocessableTrigger, message);
                return;
            }

            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger.Value))
        {
            await machine.FireAsync(trigger.Value, message);
        }
        else
        {
            await machine.FireAsync(BotTrigger.Error);
        }
    }

    public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var message = callbackQuery.EnsureExists().Message.EnsureExists();
        var chatId = message.From.EnsureExists().Id;
        var data = callbackQuery.Data.EnsureExists();
        var text = message.Text.EnsureExists();

        var session = await sessionManger.GetOrCreateSessionAsync(chatId);
        var logedUser = session.LoggenUser.EnsureExists();
        if (!await IsHasPermissionAsync(session))
        {
            return;
        }
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(logedUser.Localization.EnsureExists());

        var machine = session.CurrentStateMachine.EnsureExists();
        var triggerSrt = text.Split(':')[0];
        var trigger = ParseLocalizedBotTrigger(triggerSrt);
        if (!trigger.HasValue)
        {
            (var isReprocessable, var reprocessableTrigger) = IsStateReprocessable(machine.State);
            if (isReprocessable)
            {
                await machine.FireAsync(reprocessableTrigger, message);
                return;
            }

            await machine.FireAsync(BotTrigger.Error);
            return;
        }

        if (machine.CanFire(trigger.Value))
        {
            await machine.FireAsync(trigger.Value, data);
        }
        else
        {
            await machine.FireAsync(BotTrigger.Error);
        }
    }

    private Tuple<bool, BotTrigger> IsStateReprocessable(BotState state)
    {
        botStateMachineFactory.StateDefinition.TryGetValue(state, out var definition);
        return definition?.ShouldBeRecalled ?? Tuple.Create(false, BotTrigger.Error);
    }

    private async Task<bool> IsHasPermissionAsync(UserSession session)
    {
        var loggenUser = session.LoggenUser.EnsureExists();
        var accountId = await walletIdentityClient.GetCurrentUserByTelegramUserIdAsync(loggenUser.TelegramUserId);
        var isAccountFound = accountId != null && accountId.Id != Guid.Empty;
        if (!isAccountFound)
        {
            logger.LogError($"Account not found or don't have permission to access the account. ChatId: {session.ChatId}");
            await botClient.SendMessage(session.ChatId, "You don't have permission to access the account. Please contact the administrator.");
        }
        else
        {
            logger.LogInfo($"Account found. ChatId: {session.ChatId}, AccountId: {accountId.EnsureExists().Id}");
        }

        return isAccountFound;
    }

    private static BotTrigger? ParseLocalizedBotTrigger(string localizedText)
    {
        foreach (var trigger in Enum.GetValues<BotTrigger>())
        {
            var description = trigger.GetLocalizedDescription();
            if (string.Equals(description, localizedText, StringComparison.OrdinalIgnoreCase))
            {
                return trigger;
            }
        }

        return null;
    }
}
