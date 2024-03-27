using Wallet.Services.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureTelegramService();

var app = builder.Build();
app.Run();