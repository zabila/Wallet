using Telegram.Bot;
using TelegramService.AsyncDataServices;
using TelegramService.Contracts;
using TelegramService.Handlers;
using TelegramService.Services;
using TelegramService.SyncDataServices.Http;
using TelegramService.WalletStates;
using TelegramService.WalletStates.Incoming;
using TelegramService.WalletStates.Outcoming;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

//Get token from env variable
var telegramBotToken = Environment.GetEnvironmentVariable("TelegramBotToken");
if (string.IsNullOrEmpty(telegramBotToken))
{
    throw new ArgumentNullException(nameof(telegramBotToken));
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
builder.Services.AddScoped<IWalletState, ChooseOutcomingCategoryState>();

builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

//app.UseHttpsRedirection();

app.Run();