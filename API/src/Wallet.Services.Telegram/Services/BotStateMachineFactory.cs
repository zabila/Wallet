using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Incoming;

namespace Wallet.Services.Telegram.Services;

public class BotStateMachineFactory(ITelegramBotClient botClient, IWalletDataClient dataClient) : IBotStateMachineFactory {
    public Dictionary<BotState, IStateDefinition> StateDefinition { get; } = InitializeStateDefinitions(botClient, dataClient);

    public StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session) {
        var machine = session.CurrentStateMachine ?? new StateMachine<BotState, BotTrigger>(BotState.Idle);
        machine.OnUnhandledTrigger((state, trigger) => { });
        foreach (var definition in StateDefinition.Values) {
            definition.ConfigureState(machine, session);
        }

        return machine;
    }


    private static Dictionary<BotState, IStateDefinition> InitializeStateDefinitions(ITelegramBotClient botClient, IWalletDataClient dataClient) {
        return new Dictionary<BotState, IStateDefinition> {
            { BotState.Idle, new IdleStateDefinition(botClient) },
            { BotState.Income, new IncomeStateDefinition(botClient, dataClient) },
            { BotState.CategorySelected, new CategorySelectedStateDefinition(botClient) },
            { BotState.AmountEntered, new AmountEnteredStateDefinition(botClient) },
        };
    }
}