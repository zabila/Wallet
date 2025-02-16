using API.Telegram.Contracts;
using API.Telegram.Models;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using API.Telegram.Resources;

namespace API.Telegram.WalletStates;

public class IdleStateDefinition(ITelegramBotClient botClient) : IStateDefinition
{
    public BotState State { get; } = BotState.Idle;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(false, BotTrigger.Error);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession)
    {
        stateMachine.Configure(State)
            .PermitReentry(BotTrigger.Error)
            .Permit(BotTrigger.Income, BotState.Income)
            .Permit(BotTrigger.Expenses, BotState.Expenses)
            .OnEntryFromAsync(BotTrigger.Reset, () =>
            {
                var keyboardMarkup = CreateReplyKeyboardMarkup();
                return botClient.SendMessage(userSession.ChatId,
                    string.Format(TelegramBot.IdleStateDefinition_ConfigureState_Please_choose__0__or__1__transaction, TelegramBot.IdleStateDefinition_CreateReplyKeyboardMarkup_Income, TelegramBot.IdleStateDefinition_CreateReplyKeyboardMarkup_Expenses),
                    replyMarkup: keyboardMarkup);
            })
            .OnEntryFromAsync(BotTrigger.Error, async () =>
            {
                await botClient.SendMessage(userSession.ChatId, TelegramBot.IdleStateDefinition_ConfigureState_Invalid_input__Please_try_again);
                var keyboardMarkup = CreateReplyKeyboardMarkup();
                await botClient.SendMessage(userSession.ChatId,
                    string.Format(TelegramBot.IdleStateDefinition_ConfigureState_Please_choose__0__or__1__transaction, TelegramBot.IdleStateDefinition_CreateReplyKeyboardMarkup_Income, TelegramBot.IdleStateDefinition_CreateReplyKeyboardMarkup_Expenses),
                    replyMarkup: keyboardMarkup);
            });
    }

    private static ReplyKeyboardMarkup CreateReplyKeyboardMarkup()
    {
        return new ReplyKeyboardMarkup([[TelegramBot.IdleStateDefinition_CreateReplyKeyboardMarkup_Expenses, TelegramBot.IdleStateDefinition_CreateReplyKeyboardMarkup_Income]])
        {
            ResizeKeyboard = true
        };
    }
}
