using API.Telegram.Contracts;
using Stateless;

namespace API.Telegram.Models;

public class UserSession
{
    public required long ChatId { get; init; }
    public DateTime LastInteractionTime { get; set; } = DateTime.UtcNow;
    public StateMachine<BotState, BotTrigger>? CurrentStateMachine { get; set; }
    public List<StateHistoryItem> StateHistory { get; init; } = [];
    public Dictionary<BotState, object> StateData { get; set; } = [];

    public LoggenUser LoggenUser { get; set; }
}
