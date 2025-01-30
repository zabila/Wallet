using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Expenses;
using Wallet.Services.Telegram.WalletStates.Incoming;

namespace Wallet.Services.Telegram.Services;

public class BotStateMachineFactory : IBotStateMachineFactory {
    private readonly IMessageBusClient _messageBusClient;

    public BotStateMachineFactory(ITelegramBotClient botClient, IWalletDataClient dataClient, IMessageBusClient messageBusClient) {
        _messageBusClient = messageBusClient;
        StateDefinition = InitializeStateDefinitions(botClient, dataClient);
    }

    public Dictionary<BotState, IStateDefinition> StateDefinition { get; }

    public StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session) {
        var machine = session.CurrentStateMachine ?? new StateMachine<BotState, BotTrigger>(BotState.Idle);
        machine.OnUnhandledTrigger((state, trigger) => { });
        foreach (var definition in StateDefinition.Values) {
            definition.ConfigureState(machine, session);
        }

        return machine;
    }


    private Dictionary<BotState, IStateDefinition> InitializeStateDefinitions(ITelegramBotClient botClient, IWalletDataClient dataClient) {
        return new Dictionary<BotState, IStateDefinition> {
            { BotState.Idle, new IdleStateDefinition(botClient) },
            { BotState.Income, new IncomeStateDefinition(botClient, dataClient) },
            { BotState.IncomeCategorySelected, new IncomeCategorySelectedStateDefinition(botClient) },
            { BotState.IncomeAmountEntered, new IncomeAmountEnteredStateDefinition(botClient, _messageBusClient) },
            { BotState.Expenses, new ExpensesStateDefinition(botClient, dataClient) },
            { BotState.ExpenseCategorySelected, new ExpenseCategorySelectedStateDefinition(botClient) },
            { BotState.ExpenseAmountEntered, new ExpenseAmountEnteredStateDefinition(botClient, _messageBusClient) },
        };
    }
}