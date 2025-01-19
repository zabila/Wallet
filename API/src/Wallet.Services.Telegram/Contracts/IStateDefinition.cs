using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public enum BotState {
    Idle,
    Income,
    Expenses,
    CategorySelected,
}

public enum BotTrigger {
    Reset,
    Error,
    Income,
    Expenses,
    CategorySelected,
}

public interface IStateDefinition {
    BotState State { get; }

    void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession);
}