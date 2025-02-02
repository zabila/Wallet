using Stateless;
using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

/// <summary>
/// Represents a factory for creating and managing bot state machines
/// and providing access to state definitions.
/// </summary>
public interface IBotStateMachineFactory {
    /// <summary>
    /// Gets the dictionary of state definitions for the bot's state machine, where each
    /// <see cref="BotState"/> is mapped to its corresponding <see cref="IStateDefinition"/>.
    /// </summary>
    /// <remarks>
    /// This property allows access to the configuration and behavior rules associated with
    /// each state within the bot's state machine. The definitions include state-specific
    /// details and rules, such as whether the state should be recalled and its configuration
    /// within the state machine.
    /// </remarks>
    Dictionary<BotState, IStateDefinition> StateDefinition { get; }

    /// <summary>
    /// Creates a new instance of the bot state machine for the given user session.
    /// </summary>
    /// <param name="session">The user session for which the state machine is being created.</param>
    /// <returns>A new state machine configured with the appropriate states and triggers.</returns>
    StateMachine<BotState, BotTrigger> CreateStateMachine(UserSession session);
}