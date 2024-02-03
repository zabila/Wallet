using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramService.Abstract;

namespace TelegramService.WalletStates.Outcoming;

public class ChooseOutcomingCategoryState : WalletStateBase
{
    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        // var categories = await Mediator.Send(new GetAllCategoriesQuery(false), cancellationToken);
        // var inlineKeyboard = new InlineKeyboardMarkup(categories
        //     .Where(c => !string.IsNullOrEmpty(c))
        //     .Select(category => InlineKeyboardButton.WithCallbackData(category!, category!)));
        //
        // if (!categories.Any())
        // {
        //     Context!.SetState(new GetAmountState());
        //
        //     await BotClient.SendTextMessageAsync(
        //         chatId: message.Chat.Id,
        //         text: "Вибачте, але ви не маєте жодної категорії!!!",
        //         cancellationToken: cancellationToken);
        //
        //     await Context.HandleRequest(message, cancellationToken);
        //     return;
        // }
        //
        // await BotClient.SendTextMessageAsync(
        //     chatId: message.Chat.Id,
        //     text: "Вибери категорію:",
        //     replyMarkup: inlineKeyboard,
        //     cancellationToken: cancellationToken);
    }

    public override async Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        Context!.SetState(new SaveAmountState(callbackQuery.Data));

        await BotClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Chosen {callbackQuery.Data}",
            cancellationToken: cancellationToken);
    }
}