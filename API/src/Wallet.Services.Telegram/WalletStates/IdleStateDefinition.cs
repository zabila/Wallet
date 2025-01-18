using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.WalletStates;

public class IdleStateDefinition(ITelegramBotClient botClient) : IStateDefinition {
    public BotState State { get; } = BotState.Idle;

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(BotState.Idle)
            .OnEntryAsync(() => botClient.SendTextMessageAsync(userSession.ChatId, "Please choose Incoming or Outgoing transaction"))
            .Permit(BotTrigger.Incoming, BotState.Incoming);
    }
}