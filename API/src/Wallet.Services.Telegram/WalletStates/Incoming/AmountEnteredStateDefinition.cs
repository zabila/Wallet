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
            .OnEntryFromAsync(BotTrigger.AmountEntering, async () => {
                await botClient.SendMessage(userSession.ChatId, $"Please enter the amount");
            })
            .OnEntryFromAsync(BotTrigger.AmountEntered, async trasition => {
                await botClient.SendMessage(userSession.ChatId, $"You entered amount {trasition.Parameters[0].EnsureExists()}");
                await stateMachine.FireAsync(BotTrigger.Reset);
            });
    }
}