using API.Telegram.Contracts;
using API.Telegram.Models;
using API.Telegram.WalletStates;
using API.Telegram.WalletStates.Expenses;
using API.Telegram.WalletStates.Incoming;
using MessageBus.Publisher;
using Stateless;
using Telegram.Bot;

namespace API.Telegram.Services;

public class BotStateMachineFactory : IBotStateMachineFactory
{
    private readonly IMessageBusClient _messageBusClient;

    public BotStateMachineFactory(ITelegramBotClient botClient, IWalletFinanceAccountClient financeAccountClient, IMessageBusClient messageBusClient)
    {
        _messageBusClient = messageBusClient;
        StateDefinition = InitializeStateDefinitions(botClient, financeAccountClient);
    }

    public Dictionary<BotState, IStateDefinition> StateDefinition { get; }

    public StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session)
    {
        var machine = session.CurrentStateMachine ?? new StateMachine<BotState, BotTrigger>(BotState.Idle);
        machine.OnUnhandledTrigger((state, trigger) => { });
        foreach (var definition in StateDefinition.Values)
        {
            definition.ConfigureState(machine, session);
        }

        return machine;
    }


    private Dictionary<BotState, IStateDefinition> InitializeStateDefinitions(ITelegramBotClient botClient, IWalletFinanceAccountClient financeAccountClient)
    {
        return new Dictionary<BotState, IStateDefinition>
        {
            { BotState.Idle, new IdleStateDefinition(botClient) },
            { BotState.Income, new IncomeStateDefinition(botClient, financeAccountClient) },
            { BotState.IncomeCategorySelected, new IncomeCategorySelectedStateDefinition(botClient) },
            { BotState.IncomeAmountEntered, new IncomeAmountEnteredStateDefinition(botClient, _messageBusClient) },
            { BotState.Expenses, new ExpensesStateDefinition(botClient, financeAccountClient) },
            { BotState.ExpenseCategorySelected, new ExpenseCategorySelectedStateDefinition(botClient) },
            { BotState.ExpenseAmountEntered, new ExpenseAmountEnteredStateDefinition(botClient, _messageBusClient) }
        };
    }
}
