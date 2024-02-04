using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramService.Abstract;
using TelegramService.Dtos;
using TelegramService.Services;

namespace TelegramService.WalletStates.Incoming;

public class IncomingState : WalletStateBase
{
    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        await BotClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        var categories = await WalletDataClient.GetIncomingCategoriesAsync();
        var categoriesWithNewValue = categories.Append("New category");

        var inlineKeyboard = new InlineKeyboardMarkup(categoriesWithNewValue
            .Where(c => !string.IsNullOrEmpty(c))
            .Select(category => new[] { InlineKeyboardButton.WithCallbackData(category!, category!) }));

        await BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Choose a category:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        string? responseText = null;
        var category = callbackQuery.Data;
        if (category == "New category")
        {
            responseText = $"Chosen {callbackQuery.Data} category!\n " +
                           "Please enter template income: \n" +
                           "{Category} {Amount}";
            category = null;
        }
        else
        {
            responseText = $"Chosen {callbackQuery.Data} category!\n " +
                           "Please enter template income: \n" +
                           "{Amount}";
        }


        await BotClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: responseText,
            cancellationToken:
            cancellationToken);

        Context!.SetState(new ChosenIncomingCategoryState(category));
    }
}