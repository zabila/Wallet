using API.Identity.Extensions;
using Infrastructure;
using SharedKernel.Abstractions;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// Configure data protection to use a shared volume and encryption
builder.Services.AddDataProtection()
    .SetApplicationName("WalletApp")
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"));

var app = builder.Build();
app.MapEndpoints();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
await app.RunAsync();
