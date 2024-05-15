using NLog;
using Telegram.Bot;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Handlers;
using Wallet.Services.Telegram.Services;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Incoming;
using Wallet.Services.Telegram.WalletStates.Outcoming;

namespace Wallet.Services.Telegram.Extensions;

public static class ServiceExtensions {
    public static void ConfigureLoggerService(this IServiceCollection services) {
        LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureTelegramService(this IServiceCollection services) {
        var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        if (string.IsNullOrEmpty(telegramBotToken)) {
            throw new ArgumentNullException(nameof(telegramBotToken), "Telegram bot token is not set");
        }

        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, _) => {
                TelegramBotClientOptions options = new(telegramBotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddHttpClient<IWalletDataClient, HttpWalletDataClient>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.AddScoped<IWalletContext, WalletContext>();
        services.AddScoped<IWalletState, IncomingState>();
        services.AddScoped<IWalletState, OutcomingState>();

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    }
}