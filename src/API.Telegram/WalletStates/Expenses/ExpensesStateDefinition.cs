using API.Telegram.Contracts;
using API.Telegram.Models;
using API.Telegram.Resources;
using API.Telegram.WalletStates.Base;
using Stateless;
using Telegram.Bot;

namespace API.Telegram.WalletStates.Expenses;

public class ExpensesStateDefinition(ITelegramBotClient botClient, IWalletFinanceAccountClient financeAccountClient) : StateDefinitionBase, IStateDefinition
{
    public BotState State { get; } = BotState.Expenses;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(false, BotTrigger.Error);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession)
    {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Error, BotState.Idle)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.CategorySelected, BotState.ExpenseCategorySelected)
            .OnEntryAsync(async () =>
            {
                var categories = await financeAccountClient.GetOutcomingCategoriesAsync();
                await botClient.SendMessage(userSession.ChatId, $"{TelegramBot.ExpensesStateDefinition_ConfigureState_CategorySelected}:", replyMarkup: CreateInlineKeyboardMarkup(categories));
            });
    }
}
