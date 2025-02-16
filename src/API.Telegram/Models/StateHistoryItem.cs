using API.Telegram.Contracts;

namespace API.Telegram.Models;

public class StateHistoryItem
{
    public required BotState State { get; init; }
    public required BotTrigger Trigger { get; init; }
    public required DateTime Time { get; init; }
}
