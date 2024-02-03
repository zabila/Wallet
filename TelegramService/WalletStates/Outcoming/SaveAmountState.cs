using Telegram.Bot.Types;
using TelegramService.Abstract;

namespace TelegramService.WalletStates.Outcoming;

public class SaveAmountState(string? category = "General") : WalletStateBase
{
    private string _currentCategory = category ?? throw new ArgumentNullException(nameof(category));

    public override async Task HandleRequest(Message message, CancellationToken cancellationToken)
    {
        var command = message.Text?.Split(' ') ?? new string?[0];

        _currentCategory = (command.Length == 2 ? command[0] : _currentCategory) ?? string.Empty;
        var isAmountValid = int.TryParse(command.Length == 2 ? command[1] : command[0], out var amount);

        if (!isAmountValid)
        {
            throw new InvalidOperationException("An error occured while parsing the amount.");
        }

        if (message.From == null)
        {
            throw new InvalidOperationException("An error occured while parsing the user.");
        }

        // TransactionForCreationDto transactionDto = new()
        // {
        //     Amount = amount,
        //     Category = _currentCategory,
        //     TelegramUserId = (int)message.From.Id,
        //     Description = "No description",
        //     Name = message.Text,
        //     Type = "outcoming",
        // };
        //
        // await Mediator.Send(new CreateTransactionCommand(transactionDto), cancellationToken);

        Context!.SetState(new ChooseModeState());
        await Context!.HandleRequest(message, cancellationToken);
    }

    public override Task HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}