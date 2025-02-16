using API.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureTelegramService(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
await app.RunAsync();
