using Stateless;
using Wallet.Services.Telegram.Attributes;
using Wallet.Services.Telegram.Extensions;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public enum BotState {
    Idle,
    Income,
    Expenses,
    IncomeCategorySelected,
    IncomeAmountEntered,
    ExpenseCategorySelected,
    ExpenseAmountEntered,
}

public enum BotTrigger {
    Reset,
    Error,

    [LocalizedDescription("IdleStateDefinition_CreateReplyKeyboardMarkup_Income")]
    Income,

    [LocalizedDescription("IdleStateDefinition_CreateReplyKeyboardMarkup_Expenses")]
    Expenses,

    [LocalizedDescription("ExpensesStateDefinition_ConfigureState_CategorySelected")]
    CategorySelected,
    AmountEntering,
    AmountEntered,
    ShareLocation
}

public interface IStateDefinition {
    BotState State { get; }
    Tuple<bool, BotTrigger> ShouldBeRecalled { get; }
    void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession);
}