using System.Text.Json;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Dtos;
using Wallet.Services.Telegram.Models;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.WalletStates.Incoming;

public class IncomeAmountEnteredStateDefinition(ITelegramBotClient botClient, IMessageBusClient messageBusClient) : IStateDefinition {
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
                var replyKeyboard = new ReplyKeyboardMarkup(new[] {
                    KeyboardButton.WithRequestLocation("Share Location")
                }) {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                return botClient.SendMessage(userSession.ChatId, $"Please enter the amount for category {categories}!", replyMarkup: replyKeyboard);
            })
            .OnEntryFromAsync(BotTrigger.AmountEntered, async transition => {
                var message = (Message)transition.Parameters[0].EnsureExists();
                var messageText = message.Text.EnsureExists();
                if (!IsAmountValidAndSanitize((string)messageText, out var amount)) {
                    await botClient.SendMessage(userSession.ChatId, $"Amount {amount} is not valid. Please enter a valid amount.");
                    return;
                }

                var placeName = (string)userSession.StateData[State] ?? "Unknown Place";
                await botClient.SendMessage(userSession.ChatId, $"You entered amount {amount} for category {userSession.StateData[BotState.IncomeCategorySelected]} in {placeName}");

                var transaction = new TransactionPublishedDto() {
                    Amount = decimal.Parse((string)amount),
                    TelegramUserId = (int)userSession.ChatId,
                    Category = userSession.StateData[BotState.IncomeCategorySelected].EnsureExists().ToString(),
                    Type = "Income",
                    Location = placeName,
                    Description = "Telegram chat Transaction",
                };

                messageBusClient.PublishNewTransaction(transaction);

                userSession.StateData.Remove(State);

                await botClient.SendMessage(userSession.ChatId, $"Transaction  {transaction.Id} has been saved.");
                await stateMachine.FireAsync(BotTrigger.Reset);
            }).OnEntryFromAsync(BotTrigger.ShareLocation, async transition => {
                var message = (Message)transition.Parameters[0].EnsureExists();
                var latitude = message.Location?.Latitude;
                var longitude = message.Location?.Longitude;
                var placeName = await GetPlaceNameAsync(latitude, longitude).EnsureExists();
                userSession.StateData[State] = placeName.EnsureExists();
            });
    }

    private static bool IsAmountValidAndSanitize(string amount, out string sanitized) {
        if (string.IsNullOrWhiteSpace(amount)) {
            sanitized = "";
            return false;
        }

        sanitized = new string(amount.Where(c => !char.IsWhiteSpace(c)).ToArray());
        return sanitized.All(char.IsDigit);
    }

    private static async Task<string?> GetPlaceNameAsync(double? latitude, double? longitude) {
        if (latitude == null || longitude == null) {
            return "Unknown Place";
        }

        try {
            string url = $"https://nominatim.openstreetmap.org/reverse?lat={latitude}&lon={longitude}&format=json&addressdetails=1";
            HttpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyTelegramLocationBot/1.0");
            var response = await HttpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;
            return root.TryGetProperty("display_name", out var displayNameElement) ? displayNameElement.GetString() : "Unknown Place";
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return "Unknown Place";
        }
    }
}