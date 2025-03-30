using API.Telegram.Extensions;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

// Configure data protection to use a shared volume and encryption
builder.Services.AddDataProtection()
    .SetApplicationName("WalletApp")
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));

builder.Services.ConfigureLoggerService();
builder.Services.ConfigureTelegramService(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
await app.RunAsync();
