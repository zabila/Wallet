using Stateless;
using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Models;

public class UserSession {
    public required long ChatId { get; init; }
    public DateTime LastInteractionTime { get; set; } = DateTime.UtcNow;
    public StateMachine<BotState, BotTrigger> CurrentStateMachine { get; set; } = new StateMachine<BotState, BotTrigger>(BotState.Idle);
    public List<StateHistoryItem> StateHistory { get; init; } = [];
    public Dictionary<BotState, object> StateData { get; set; } = [];
}