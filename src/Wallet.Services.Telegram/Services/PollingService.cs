using Wallet.Services.Telegram.Abstract;
using Wallet.Services.Telegram.Contracts;

namespace Wallet.Services.Telegram.Services;

public class PollingService(IServiceProvider serviceProvider, ILoggerManager logger) : PollingServiceBase<ReceiverService>(serviceProvider, logger);