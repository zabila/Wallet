using API.Telegram.Abstract;
using SharedKernel.Abstractions;
using API.Telegram.Contracts;

namespace API.Telegram.Services;

public class PollingService(IServiceProvider serviceProvider, ILoggerManager logger) : PollingServiceBase<ReceiverService>(serviceProvider, logger);
