using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public interface IBotStateMachineFactory {
    StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session);
}