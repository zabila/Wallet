using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates.Base;

namespace Wallet.Services.Telegram.WalletStates.Incoming;

public class IncomeStateDefinition(ITelegramBotClient botClient, IWalletDataClient dataClient) : StateDefinitionBase, IStateDefinition {
    public BotState State { get; } = BotState.Income;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(false, BotTrigger.Error);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Error, BotState.Idle)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.CategorySelected, BotState.IncomeCategorySelected)
            .OnEntryAsync(async () => {
                var categories = await dataClient.GetIncomingCategoriesAsync();
                await botClient.SendMessage(userSession.ChatId, $"{nameof(BotTrigger.CategorySelected)}:", replyMarkup: CreateInlineKeyboardMarkup(categories));
            });
    }
}