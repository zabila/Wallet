using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Incoming;

namespace Wallet.Services.Telegram.Services;

public class BotStateMachineFactory(ITelegramBotClient botClient) : IBotStateMachineFactory {
    public StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session) {
        var machine = session.CurrentStateMachine;
        var stateDefinition = GetStateDefinition(machine.State);
        foreach (var definition in stateDefinition) {
            definition.ConfigureState(machine, session);
        }

        return machine;
    }


    private IEnumerable<IStateDefinition> GetStateDefinition(BotState state) {
        return [
            new IdleStateDefinition(botClient),
            new IncomingStateDefinition(botClient)
        ];
    }
}