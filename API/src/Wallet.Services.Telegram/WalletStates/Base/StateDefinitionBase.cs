using Telegram.Bot.Types.ReplyMarkups;

namespace Wallet.Services.Telegram.WalletStates.Base;

public abstract class StateDefinitionBase {
    protected InlineKeyboardMarkup CreateInlineKeyboardMarkup(IEnumerable<string> categories) {
        return new InlineKeyboardMarkup(
            categories
                .Chunk(3)
                .Select(chunk => chunk
                    .Select(category => InlineKeyboardButton.WithCallbackData(category, $"{category}"))
                    .ToArray())
                .ToArray());
    }
}