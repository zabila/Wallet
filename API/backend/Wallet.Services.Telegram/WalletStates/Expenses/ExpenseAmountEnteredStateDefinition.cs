using System.Text.Json;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Dtos;
using Wallet.Services.Telegram.Models;
using Wallet.Services.Telegram.WalletStates.Base;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Expenses;

public class ExpenseAmountEnteredStateDefinition(ITelegramBotClient botClient, IMessageBusClient messageBusClient) : AmountEnteredStateDefinitionBase, IStateDefinition {
    public BotState State { get; } = BotState.ExpenseAmountEntered;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(true, BotTrigger.AmountEntered);

    private static readonly HttpClient HttpClient = new HttpClient();

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession) {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.Error, BotState.Idle)
            .PermitReentry(BotTrigger.AmountEntered)
            .PermitReentry(BotTrigger.ShareLocation)
            .OnEntryFromAsync(BotTrigger.AmountEntering, () => {
                var categories = userSession.StateData[BotState.ExpenseCategorySelected].EnsureExists();
                return botClient.SendMessage(userSession.ChatId, $"Please enter the amount for category {categories}!", replyMarkup: CreateReplyKeyboardMarkup());
            })
            .OnEntryFromAsync(BotTrigger.AmountEntered, async transition => {
                var message = (Message)transition.Parameters[0].EnsureExists();
                var amount = GetAmount(message);
                if (!amount.HasValue) {
                    await botClient.SendMessage(userSession.ChatId, $"Amount {amount} is not valid. Please enter a valid amount.");
                    return;
                }

                var placeName = (string)userSession.StateData[State] ?? "Unknown Place";
                await botClient.SendMessage(userSession.ChatId, $"You entered amount {amount} for category {userSession.StateData[BotState.ExpenseCategorySelected]} in {placeName}");

                var transaction = new TransactionPublishedDto() {
                    Amount = amount.Value,
                    TelegramUserId = (int)userSession.ChatId,
                    Category = userSession.StateData[BotState.ExpenseCategorySelected].EnsureExists().ToString(),
                    Type = "Outcome",
                    Location = placeName,
                    Description = "Telegram chat Transaction",
                };

                messageBusClient.PublishNewTransaction(transaction);

                userSession.StateData.Remove(State);
                await botClient.SendMessage(userSession.ChatId, $"Transaction  {transaction.Id} has been saved.");
                await stateMachine.FireAsync(BotTrigger.Reset);
            }).OnEntryFromAsync(BotTrigger.ShareLocation, transition => {
                var message = (Message)transition.Parameters[0].EnsureExists();
                var latitude = message.Location?.Latitude;
                var longitude = message.Location?.Longitude;
                userSession.StateData[State] = $"location: {latitude}, {longitude}";
                return Task.CompletedTask;
            });
    }
}