using System.Collections;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;
using Telegram.Bot;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Handlers;
using Wallet.Services.Telegram.Services;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Incoming;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.Extensions;

public static class ServiceExtensions {
    public static void ConfigureLoggerService(this IServiceCollection services) {
        LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureTelegramService(this IServiceCollection services, IConfiguration configuration) {
        var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        if (string.IsNullOrEmpty(telegramBotToken)) {
            throw new ArgumentNullException(nameof(telegramBotToken), "Telegram bot token is not set");
        }

        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, _) => {
                TelegramBotClientOptions options = new(telegramBotToken);
                return new TelegramBotClient(options, httpClient);
            });

        var apiFinancialUrl = configuration["WalletFinanceAccountApi"].EnsureExists();
        var apiIdentityUrl = configuration["WalletIdentityApi"].EnsureExists();
        services.AddSingleton<IFlurlClientCache>(sp => {
            var cache = new FlurlClientCache();
            cache.Add(nameof(HttpWalletFinanceAccountClient), apiFinancialUrl);
            cache.Add(nameof(HttpWalletIdentityClient), apiIdentityUrl);
            return cache;
        });

        services.AddSingleton<IWalletIdentityClient, HttpWalletIdentityClient>();
        services.AddSingleton<IWalletFinanceAccountClient, HttpWalletFinanceAccountClient>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.AddScoped<IWalletContext, WalletContext>();

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();

        services.AddScoped<ISessionManager, InMemorySessionManager>();
        services.AddScoped<IBotStateMachineFactory, BotStateMachineFactory>();

        services.AddScoped<IStateDefinition, IncomeStateDefinition>();
    }
}