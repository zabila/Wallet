using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

/// <summary>
/// Represents the various states of a bot within its state machine.
/// </summary>
public enum BotState {
    /// <summary>
    /// Represents the initial or resting state of the bot before any user interaction or operation has commenced.
    /// </summary>
    Idle,

    /// <summary>
    /// Represents the bot state where it processes and manages income-related actions or data.
    /// </summary>
    Income,

    /// <summary>
    /// Represents the state where the bot is handling expense-related operations.
    /// Typically, this state is active when the user initiates actions related to tracking or managing expenses.
    /// </summary>
    Expenses,

    /// <summary>
    /// Represents the state of the bot where a specific category has been selected by the user.
    /// </summary>
    IncomeCategorySelected,

    /// <summary>
    /// Indicates the state in which the bot expects the user to have entered an amount,
    /// typically in the context of recording a financial transaction or similar action.
    /// </summary>
    IncomeAmountEntered,
    ExpenseCategorySelected,
    ExpenseAmountEntered,
}

/// <summary>
/// Represents a set of triggers that can invoke transitions between states in a bot's state machine.
/// </summary>
public enum BotTrigger {
    /// <summary>
    /// Represents a trigger that resets the bot to its initial state, typically used to clear any ongoing state or transition.
    /// </summary>
    Reset,

    /// <summary>
    /// Represents a trigger for handling unexpected or error scenarios within the bot's state machine.
    /// </summary>
    Error,

    /// <summary>
    /// Represents the trigger that transitions the bot state machine to manage income-related actions.
    /// </summary>
    Income,

    /// <summary>
    /// Represents the bot's mechanism to handle the "expenses" trigger, used to transition
    /// the bot's state when recording or managing expense-related interactions.
    /// </summary>
    Expenses,

    /// <summary>
    /// Represents the trigger for transitioning to the "IncomeCategorySelected" state, where a user selects a category.
    /// </summary>
    CategorySelected,

    /// <summary>
    /// Represents a trigger within the bot's state machine that initiates the process
    /// of entering an amount for a specific category or operation.
    /// </summary>
    AmountEntering,

    /// <summary>
    /// Represents the trigger that is fired when
    /// </summary>
    AmountEntered,

    /// <summary>
    /// Share location trigger
    /// </summary>
    ShareLocation
}

/// <summary>
/// Defines the structure for state definitions used within the bot's state machine.
/// </summary>
public interface IStateDefinition {
    /// <summary>
    /// Gets the current state associated with the state machine configuration.
    /// Represents a specific stage or condition in the bot's workflow.
    /// </summary>
    BotState State { get; }

    /// <summary>
    /// Gets a tuple indicating whether the current state should be recalled
    /// and the specific trigger to be invoked if true.
    /// </summary>
    /// <remarks>
    /// - The first item in the tuple is a boolean indicating if the state is reprocessable.
    /// - The second item specifies the trigger that should be executed if the state is recalled.
    /// This property is used to determine if a state requires further processing or handling
    /// within the bot's state machine.
    /// </remarks>
    Tuple<bool, BotTrigger> ShouldBeRecalled { get; }

    /// <summary>
    /// Configures the bot state machine with specific transitions and actions associated with the current state.
    /// </summary>
    /// <param name="stateMachine">The state machine instance to configure for the bot.</param>
    /// <param name="userSession">The user session object containing context and state data for the user.</param>
    void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession);
}