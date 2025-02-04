using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.Resources;
using Wallet.Shared.DataTransferObjects;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Base;

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
        var latitude = message.Location?.Latitude;
        var longitude = message.Location?.Longitude;
        var location = new LocationDto()
        {
            Latitude = decimal.Parse(latitude?.ToString() ?? "0"),
            Longitude = decimal.Parse(longitude?.ToString() ?? "0")
        };

        userSession.StateData[state] = location;
        return true;
    }

    protected static LocationDto? GetLocation(UserSession userSession, BotState state)
    {
        userSession.StateData.TryGetValue(state, out var placeNameObj);
        var placeName = placeNameObj as LocationDto ?? new LocationDto();
        return placeName;
    }
}
