using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.WalletStates.Incoming;

public class IncomingStateDefinition(ITelegramBotClient botClient) : IStateDefinition {
    public BotState State { get; } = BotState.Incoming;

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(BotState.Incoming)
            .OnEntryAsync(() => botClient.SendTextMessageAsync(userSession.ChatId, "You are in Incoming state, In feature you should choose category of incoming transaction"));
    }
}