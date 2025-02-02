using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Models;

public class StateHistoryItem {
    public required BotState State { get; init; }
    public required BotTrigger Trigger { get; init; }
    public required DateTime Time { get; init; }
}