using Microsoft.AspNetCore.Identity;
using Wallet.Infrastructure.LoggerService;
using Microsoft.EntityFrameworkCore;
using NLog;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Model;
using Wallet.Infrastructure.Repository;

namespace Wallet.API.Identity.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }
    
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder();
        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<RepositoryContext>()            
            .AddDefaultTokenProviders();
    }
}