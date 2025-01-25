using Stateless;
using Telegram.Bot;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Incoming;

public class AmountEnteredStateDefinition(ITelegramBotClient botClient) : IStateDefinition {
    public BotState State { get; } = BotState.AmountEntered;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(true, BotTrigger.AmountEntered);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.Error, BotState.Idle)
            .PermitReentry(BotTrigger.AmountEntered)
            .OnEntryFromAsync(BotTrigger.AmountEntering, () => {
                var categories = userSession.StateData[BotState.CategorySelected].EnsureExists();
                return botClient.SendMessage(userSession.ChatId, $"Please enter the amount for category {categories}");
            })
            .OnEntryFromAsync(BotTrigger.AmountEntered, async trasition => {
                var amount = trasition.Parameters[0].EnsureExists();
                userSession.StateData[State] = amount;

                await botClient.SendMessage(userSession.ChatId, $"You entered amount {amount} for category {userSession.StateData[BotState.CategorySelected]}");
                await stateMachine.FireAsync(BotTrigger.Reset);
            });
    }
}