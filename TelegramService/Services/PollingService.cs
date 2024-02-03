using TelegramService.Abstract;
using TelegramService.Contracts;

namespace TelegramService.Services;

public class PollingService(IServiceProvider serviceProvider, ILoggerManager logger) : PollingServiceBase<ReceiverService>(serviceProvider, logger);