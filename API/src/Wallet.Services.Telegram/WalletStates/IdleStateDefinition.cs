using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.WalletStates;

public class IdleStateDefinition(ITelegramBotClient botClient) : IStateDefinition {
    public BotState State { get; } = BotState.Idle;

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .PermitReentry(BotTrigger.Error)
            .Permit(BotTrigger.Income, BotState.Income)
            .OnEntryAsync(() => {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new KeyboardButton[] { "Expenses", "Income" }) {
                    ResizeKeyboard = true
                };

                return botClient.SendMessage(userSession.ChatId, $"Please choose {BotTrigger.Income} or {BotTrigger.Expenses} transaction", replyMarkup: replyKeyboardMarkup);
            });
    }
}