using API.Telegram.Contracts;
using API.Telegram.Models;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using API.Telegram.Resources;
using SharedKernel.Extensions;

namespace API.Telegram.WalletStates.Incoming;

public class IncomeCategorySelectedStateDefinition(ITelegramBotClient botClient) : IStateDefinition
{
    public BotState State { get; } = BotState.IncomeCategorySelected;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(false, BotTrigger.Error);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession)
    {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.Error, BotState.Idle)
            .Permit(BotTrigger.AmountEntering, BotState.IncomeAmountEntered)
            .OnEntryFromAsync(BotTrigger.CategorySelected, async transition =>
            {
                var categories = (string)transition.Parameters[0].EnsureExists();
                userSession.StateData[State] = categories;

                await botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.ExpenseCategorySelectedStateDefinition_ConfigureState_You_selected_category__0__, categories));
                await stateMachine.FireAsync(BotTrigger.AmountEntering);
            });
    }
}
