using API.Telegram.Contracts;
using API.Telegram.Handlers;
using API.Telegram.Services;
using API.Telegram.SyncDataServices.Http;
using API.Telegram.WalletStates;
using API.Telegram.WalletStates.Incoming;
using Flurl.Http.Configuration;
using MessageBus.Publisher;
using NLog;
using SharedKernel;
using SharedKernel.Abstractions;
using SharedKernel.Extensions;
using Telegram.Bot;

namespace API.Telegram.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureTelegramService(this IServiceCollection services, IConfiguration configuration)
    {
        var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN").EnsureExists();
        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
            {
                TelegramBotClientOptions options = new(telegramBotToken);
                return new TelegramBotClient(options, httpClient);
            });

        var apiFinancialUrl = configuration["WalletFinanceAccountApi"].EnsureExists();
        var apiIdentityUrl = configuration["WalletIdentityApi"].EnsureExists();
        services.AddSingleton<IFlurlClientCache>(sp =>
        {
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
