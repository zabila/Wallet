using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Base;

public abstract class AmountEnteredStateDefinitionBase {
    protected ReplyKeyboardMarkup CreateReplyKeyboardMarkup() {
        return new ReplyKeyboardMarkup(new[] {
            KeyboardButton.WithRequestLocation("Share Location")
        }) {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    protected static bool IsAmountValidAndSanitize(string amount, out string sanitized) {
        if (string.IsNullOrWhiteSpace(amount)) {
            sanitized = "";
            return false;
        }

        sanitized = new string(amount.Where(c => !char.IsWhiteSpace(c)).ToArray());
        return sanitized.All(char.IsDigit);
    }

    protected static decimal? GetAmount(Message message) {
        var messageText = message.Text.EnsureExists();
        return !IsAmountValidAndSanitize((string)messageText, out var amount) ? 0 : decimal.Parse(amount);
    }
}