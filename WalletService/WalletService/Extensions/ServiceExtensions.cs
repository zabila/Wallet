using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Entities.Model;
using Repository;

namespace WalletService.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddApiEndpoints();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            s.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });
        });
    }
}