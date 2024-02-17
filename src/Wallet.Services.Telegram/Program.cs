using Telegram.Bot;
using Wallet.Services.Telegram.AsyncDataServices;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Handlers;
using Wallet.Services.Telegram.Services;
using Wallet.Services.Telegram.SyncDataServices.Http;
using Wallet.Services.Telegram.WalletStates;
using Wallet.Services.Telegram.WalletStates.Incoming;
using Wallet.Services.Telegram.WalletStates.Outcoming;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

//Get token from env variable
var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
if (string.IsNullOrEmpty(telegramBotToken))
{
    throw new ArgumentNullException(nameof(telegramBotToken), "Telegram bot token is not set");
}

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
    {
        TelegramBotClientOptions options = new(telegramBotToken);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddHttpClient<IWalletDataClient, HttpWalletDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddScoped<IWalletContext, WalletContext>();
builder.Services.AddScoped<IWalletState, IncomingState>();
builder.Services.AddScoped<IWalletState, OutcomingState>();

builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();

var app = builder.Build();
app.Run();