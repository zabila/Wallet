using Telegram.Bot;
using TelegramService.Abstract;
using TelegramService.Contracts;
using TelegramService.Handlers;

namespace TelegramService.Services;

public class ReceiverService(
    ITelegramBotClient botClient,
    UpdateHandler updateHandler,
    ILoggerManager logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);