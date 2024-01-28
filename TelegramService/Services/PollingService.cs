using TelegramService.Abstract;
using TelegramService.Contracts;

namespace TelegramService.Services;

public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILoggerManager logger)
        : base(serviceProvider, logger)
    {
    }
}