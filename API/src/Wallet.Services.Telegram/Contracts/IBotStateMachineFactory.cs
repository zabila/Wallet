using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public interface IBotStateMachineFactory {
    Dictionary<BotState, IStateDefinition> StateDefinition { get; }
    StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session);
}