using API.Telegram.Abstract;
using SharedKernel.Abstractions;

namespace API.Telegram.Services;

public class PollingService(IServiceProvider serviceProvider, ILoggerManager logger) : PollingServiceBase<ReceiverService>(serviceProvider, logger);
