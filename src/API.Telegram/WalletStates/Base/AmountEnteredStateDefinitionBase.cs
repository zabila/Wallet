using API.Telegram.Contracts;
using API.Telegram.Models;
using API.Telegram.Resources;
using SharedKernel.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace API.Telegram.WalletStates.Base;

public abstract class AmountEnteredStateDefinitionBase
{
    protected static ReplyKeyboardMarkup CreateReplyKeyboardMarkup()
    {
        return new ReplyKeyboardMarkup(KeyboardButton.WithRequestLocation(TelegramBot.AmountEnteredStateDefinitionBase_CreateReplyKeyboardMarkup_Share_Location))
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    private static bool IsAmountValidAndSanitize(string amount, out string sanitized)
    {
        if (string.IsNullOrWhiteSpace(amount))
        {
            sanitized = "";
            return false;
        }

        sanitized = new string(amount.Where(c => !char.IsWhiteSpace(c)).ToArray());
        return sanitized.All(char.IsDigit);
    }

    protected static decimal? GetAmount(Message message)
    {
        var messageText = message.Text.EnsureExists();
        return !IsAmountValidAndSanitize(messageText, out var amount) ? 0 : decimal.Parse(amount);
    }

    protected static bool SaveLocation(UserSession userSession, Message message, BotState state)
    {
        userSession.StateData[state] = message.Location ?? new Location();
        return true;
    }

    protected static Location GetLocation(UserSession userSession, BotState state)
    {
        userSession.StateData.TryGetValue(state, out var locationObj);
        var location = locationObj as Location ?? new Location();
        return location;
    }
}
