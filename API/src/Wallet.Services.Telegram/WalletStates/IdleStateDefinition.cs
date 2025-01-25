using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.WalletStates;

public class IdleStateDefinition(ITelegramBotClient botClient) : IStateDefinition {
    public BotState State { get; } = BotState.Idle;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(false, BotTrigger.Error);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .PermitReentry(BotTrigger.Error)
            .Permit(BotTrigger.Income, BotState.Income)
            .OnEntryFromAsync(BotTrigger.Reset, () => {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new KeyboardButton[] { "Expenses", "Income" }) {
                    ResizeKeyboard = true
                };

                return botClient.SendMessage(userSession.ChatId, $"Please choose {BotTrigger.Income} or {BotTrigger.Expenses} transaction", replyMarkup: replyKeyboardMarkup);
            })
            .OnEntryFromAsync(BotTrigger.Error, async () => {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new KeyboardButton[] { "Expenses", "Income" }) {
                    ResizeKeyboard = true
                };

                await botClient.SendMessage(userSession.ChatId, "Invalid input. Please try again", replyMarkup: replyKeyboardMarkup);
                await botClient.SendMessage(userSession.ChatId, $"Please choose {BotTrigger.Income} or {BotTrigger.Expenses} transaction", replyMarkup: replyKeyboardMarkup);
            });
    }
}