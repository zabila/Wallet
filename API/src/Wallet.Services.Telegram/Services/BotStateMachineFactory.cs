using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Incoming;

namespace Wallet.Services.Telegram.Services;

public class BotStateMachineFactory(ITelegramBotClient botClient, IWalletDataClient dataClient) : IBotStateMachineFactory {
    public StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session) {
        var machine = session.CurrentStateMachine ?? new StateMachine<BotState, BotTrigger>(BotState.Idle);
        machine.OnUnhandledTrigger((state, trigger) => { });
        var stateDefinition = GetStateDefinition(machine.State);
        foreach (var definition in stateDefinition) {
            definition.ConfigureState(machine, session);
        }

        return machine;
    }


    private IEnumerable<IStateDefinition> GetStateDefinition(BotState state) {
        return [
            new IdleStateDefinition(botClient),
            new IncomeStateDefinition(botClient, dataClient),
            new CategorySelectedStateDefinition(botClient)
        ];
    }
}