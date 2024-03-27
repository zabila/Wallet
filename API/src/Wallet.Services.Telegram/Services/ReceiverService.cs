using Telegram.Bot;
using Wallet.Services.Telegram.Abstract;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Handlers;

namespace Wallet.Services.Telegram.Services;

public class ReceiverService(
    ITelegramBotClient botClient,
    UpdateHandler updateHandler,
    ILoggerManager logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);