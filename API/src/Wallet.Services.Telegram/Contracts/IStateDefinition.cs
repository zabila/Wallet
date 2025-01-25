using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public enum BotState {
    Idle,
    Income,
    Expenses,
    CategorySelected,
    AmountEntered,
}

public enum BotTrigger {
    Reset,
    Error,
    Income,
    Expenses,
    CategorySelected,
    AmountEntering,
    AmountEntered,
}

public interface IStateDefinition {
    BotState State { get; }
    Tuple<bool, BotTrigger> ShouldBeRecalled { get; }

    void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession);
}