using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Wallet.API.Identity.Services;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Model;
using Wallet.Infrastructure.LoggerService;
using Wallet.Infrastructure.Repository;

namespace Wallet.API.Identity.Extensions;

public static class ServiceExtensions {
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration) {
        services.ConfigureLoggerService();
        services.ConfigureDataBase(configuration);
        services.ConfigureSwagger();
        services.ConfigureIdentity(configuration);
    }

    private static void ConfigureLoggerService(this IServiceCollection services) {
        LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    private static void ConfigureDataBase(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    private static void ConfigureSwagger(this IServiceCollection services) {
        services.AddSwaggerGen(s => {
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
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

    private static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration) {
        services.AddIdentity<WalletIdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<IEmailSender<WalletIdentityUser>, EmailSender>();

        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

        services.AddAuthorization();
    }
}