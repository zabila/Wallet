using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public enum BotState {
    Idle,
    Incoming,
    Outcoming,
}

public enum BotTrigger {
    Incoming,
    Outcoming,
    Confirm,
}

public interface IStateDefinition {
    BotState State { get; }

    void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession);
}