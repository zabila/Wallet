using API.Telegram.Contracts;
using API.Telegram.Models;
using API.Telegram.Resources;
using API.Telegram.WalletStates.Base;
using MessageBus.Events;
using MessageBus.Publisher;
using SharedKernel.Extensions;
using Stateless;
using Telegram.Bot;
using Telegram.Bot.Types;
using Location = Domain.Transactions.Location;

namespace API.Telegram.WalletStates.Incoming;

public class IncomeAmountEnteredStateDefinition(ITelegramBotClient botClient, IMessageBusClient messageBusClient) : AmountEnteredStateDefinitionBase, IStateDefinition
{
    public BotState State { get; } = BotState.IncomeAmountEntered;
    public Tuple<bool, BotTrigger> ShouldBeRecalled { get; } = Tuple.Create(true, BotTrigger.AmountEntered);

    public void ConfigureState(StateMachine<BotState, BotTrigger> stateMachine, UserSession userSession)
    {
        stateMachine.Configure(State)
            .Permit(BotTrigger.Reset, BotState.Idle)
            .Permit(BotTrigger.Error, BotState.Idle)
            .PermitReentry(BotTrigger.AmountEntered)
            .PermitReentry(BotTrigger.ShareLocation)
            .OnEntryFromAsync(BotTrigger.AmountEntering, () =>
            {
                var categories = userSession.StateData[BotState.IncomeCategorySelected].EnsureExists();
                return botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.ExpenseAmountEnteredStateDefinition_ConfigureState_Please_enter_the_amount_for_category__0__, categories), replyMarkup: CreateReplyKeyboardMarkup());
            })
            .OnEntryFromAsync(BotTrigger.AmountEntered, async transition =>
            {
                var message = (Message)transition.Parameters[0].EnsureExists();
                var amount = GetAmount(message);
                if (amount is null or <= 0)
                {
                    var text = message.Text.EnsureExists();
                    await botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.IncomeAmountEnteredStateDefinition_ConfigureState_Amount__0__is_not_valid__Please_enter_a_valid_amount_, text));
                    return;
                }

                var category = (string)userSession.StateData[BotState.IncomeCategorySelected].EnsureExists();

                await botClient.SendMessage(userSession.ChatId, string.Format(TelegramBot.ExpenseAmountEnteredStateDefinition_ConfigureState_You_entered_amount__0__for_category__1_, amount, category));

                var location = GetLocation(userSession, State);
                var transaction = new CreateTransactionEvent
                {
                    Amount = amount.Value,
                    AccountId = userSession.LoggenUser.AccoundId,
                    UserId = userSession.LoggenUser.UserId,
                    Category = category ?? "Unknown",
                    Type = "Income",
                    Location = new Location
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    }
                };

                await messageBusClient.PublishCreateTransactionEventAsync(transaction);

                userSession.StateData.Remove(State);
                await stateMachine.FireAsync(BotTrigger.Reset);
            }).OnEntryFromAsync(BotTrigger.ShareLocation, transition =>
            {
                var message = (Message)transition.Parameters[0].EnsureExists();
                SaveLocation(userSession, message, State);
                return Task.CompletedTask;
            });
    }
}
