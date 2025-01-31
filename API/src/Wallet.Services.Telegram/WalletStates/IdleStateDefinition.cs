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
            .Permit(BotTrigger.Expenses, BotState.Expenses)
            .OnEntryFromAsync(BotTrigger.Reset, () => {
                var keyboardMarkup = CreateReplyKeyboardMarkup();
                return botClient.SendMessage(userSession.ChatId, $"Please choose {BotTrigger.Income} or {BotTrigger.Expenses} transaction", replyMarkup: keyboardMarkup);
            })
            .OnEntryFromAsync(BotTrigger.Error, async () => {
                await botClient.SendMessage(userSession.ChatId, "Invalid input. Please try again");
                var keyboardMarkup = CreateReplyKeyboardMarkup();
                await botClient.SendMessage(userSession.ChatId, $"Please choose {BotTrigger.Income} or {BotTrigger.Expenses} transaction", replyMarkup: keyboardMarkup);
            });
    }

    private static ReplyKeyboardMarkup CreateReplyKeyboardMarkup() {
        return new ReplyKeyboardMarkup([
            ["Expenses", "Income"]
        ]) {
            ResizeKeyboard = true
        };
    }
}