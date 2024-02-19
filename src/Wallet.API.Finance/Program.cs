using Wallet.API.Finance.Extensions;
using Wallet.Domain.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Wallet.Application.Finance.AssemblyReference).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Wallet.Integration.MessageBus.AssemblyReference).Assembly));

builder.Services.ConfigureDataBase(builder.Configuration);
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureMessageBus();
builder.Services.ConfigureSwagger();

var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

app.UseAuthentication();    
app.UseAuthorization();
app.MapControllers();
app.Run();