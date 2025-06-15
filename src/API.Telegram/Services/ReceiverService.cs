using API.Telegram.Abstract;
using API.Telegram.Handlers;
using SharedKernel.Abstractions;
using Telegram.Bot;

namespace API.Telegram.Services;

public class ReceiverService(ITelegramBotClient botClient,
    UpdateHandler updateHandler,
    ILoggerManager logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);
