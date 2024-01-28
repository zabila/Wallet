using Telegram.Bot;
using TelegramService.Abstract;
using TelegramService.Contracts;
using TelegramService.Handlers;

namespace TelegramService.Services;

public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILoggerManager logger)
        : base(botClient, updateHandler, logger)
    {
    }
}