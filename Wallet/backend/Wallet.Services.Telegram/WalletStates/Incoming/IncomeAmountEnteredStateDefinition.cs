using System.Text.Json;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Dtos;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.Resources;
using Wallet.Services.Telegram.WalletStates.Base;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Incoming;

public class IncomeAmountEnteredStateDefinition(ITelegramBotClient botClient, IMessageBusClient messageBusClient) : AmountEnteredStateDefinitionBase, IStateDefinition {
    public BotState State { get; } = BotState.IncomeAmountEntered;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(true, BotTrigger.AmountEntered);

    private static readonly HttpClient HttpClient = new HttpClient();

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.Error, BotState.Idle)
            .PermitReentry(BotTrigger.AmountEntered)
            .PermitReentry(BotTrigger.ShareLocation)
            .OnEntryFromAsync(BotTrigger.AmountEntering, () => {
                var categories = userSession.StateData[BotState.IncomeCategorySelected].EnsureExists();
                return botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.ExpenseAmountEnteredStateDefinition_ConfigureState_Please_enter_the_amount_for_category__0__, categories), replyMarkup: CreateReplyKeyboardMarkup());
            })
            .OnEntryFromAsync(BotTrigger.AmountEntered, async transition => {
                var message = (Message)transition.Parameters[0].EnsureExists();
                var amount = GetAmount(message);
                if (amount is null or <= 0) {
                    var text = message.Text.EnsureExists();
                    await botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.IncomeAmountEnteredStateDefinition_ConfigureState_Amount__0__is_not_valid__Please_enter_a_valid_amount_, text));
                    return;
                }

                await botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.ExpenseAmountEnteredStateDefinition_ConfigureState_You_entered_amount__0__for_category__1_, amount, userSession.StateData[BotState.IncomeCategorySelected]));

                var transaction = new TransactionPublishedDto() {
                    Amount = amount.Value,
                    TelegramUserId = (int)userSession.ChatId,
                    Category = userSession.StateData[BotState.IncomeCategorySelected].EnsureExists().ToString(),
                    Type = "Income",
                    Location = GetLocation(userSession, State),
                    Description = "Telegram chat Transaction",
                };

                messageBusClient.PublishNewTransaction(transaction);
                userSession.StateData.Remove(State);
                await botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.IncomeAmountEnteredStateDefinition_ConfigureState_Transaction___0__has_been_saved_, transaction.Id));
                await stateMachine.FireAsync(BotTrigger.Reset);
            }).OnEntryFromAsync(BotTrigger.ShareLocation, transition => {
                var message = (Message)transition.Parameters[0].EnsureExists();
                SaveLocation(userSession, message, State);
                return Task.CompletedTask;
            });
    }
}