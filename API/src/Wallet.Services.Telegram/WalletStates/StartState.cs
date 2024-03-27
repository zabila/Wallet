using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Wallet.Services.Telegram.Abstract;
using Wallet.Services.Telegram.WalletStates.Incoming;
using Wallet.Services.Telegram.WalletStates.Outcoming;

namespace Wallet.Services.Telegram.WalletStates;

public class StartState : WalletStateBase
{
    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        var command = message.Text;
        switch (command)
        {
            case "Expenses":
                Context!.SetState(new OutcomingState());
                await Context!.HandleRequest(message, cancellationToken);
                return;
            case "Income":
                Context!.SetState(new IncomingState());
                await Context!.HandleRequest(message, cancellationToken);
                return;
        }

        await BotClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        ReplyKeyboardMarkup replyKeyboardMarkup = new(new KeyboardButton[] { "Expenses", "Income" })
        {
            ResizeKeyboard = true
        };

        await BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Choose an action:",
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {   
        throw new NotImplementedException();    
    }
}