using Wallet.Services.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureTelegramService(builder.Configuration);

var app = builder.Build();
await app.RunAsync();
