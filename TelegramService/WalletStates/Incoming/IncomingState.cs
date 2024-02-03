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


        string[] categories =
        [
            "Salary",
            "Gift",
            "Other"
        ];

        var inlineKeyboard = new InlineKeyboardMarkup(categories
            .Where(c => !string.IsNullOrEmpty(c))
            .Select(category => InlineKeyboardButton.WithCallbackData(category!, category!)));

        await BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Choose a category:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        Context!.SetState(new ChooseIncomingCategoryState( /*callbackQuery.Data*/));

        await BotClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Chosen {callbackQuery.Data}",
            cancellationToken: cancellationToken);
    }
}