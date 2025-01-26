using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Incoming;

public class CategorySelectedStateDefinition(ITelegramBotClient botClient) : IStateDefinition {
    public BotState State { get; } = BotState.CategorySelected;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(false, BotTrigger.Error);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.Error, BotState.Idle)
            .Permit(BotTrigger.AmountEntering, BotState.AmountEntered)
            .OnEntryFromAsync(BotTrigger.CategorySelected, async transition => {
                var categories = (string)transition.Parameters[0].EnsureExists();
                await botClient.SendMessage(userSession.ChatId, $"You selected category {categories}");
                userSession.StateData[State] = categories;
                await stateMachine.FireAsync(BotTrigger.AmountEntering);
            });
    }
}