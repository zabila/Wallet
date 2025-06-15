using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure;
using Infrastructure.Abstractions;
using Infrastructure.Database;
using Application.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace API.Finance.IntegrationTests.Common;

public class FinanceWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add test configuration
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["Jwt:ExpiryInMinutes"] = "60"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove ALL Entity Framework related services
            var entitiesToRemove = services
                .Where(descriptor => descriptor.ServiceType.Namespace != null &&
                                   (descriptor.ServiceType.Namespace.Contains("EntityFramework") ||
                                    descriptor.ServiceType == typeof(RepositoryContext) ||
                                    descriptor.ServiceType == typeof(IRepositoryContext) ||
                                    descriptor.ServiceType == typeof(IRepositoryManager) ||
                                    descriptor.ServiceType == typeof(DbContextOptions<RepositoryContext>)))
                .ToList();

            foreach (var entity in entitiesToRemove)
            {
                services.Remove(entity);
            }

            // Remove authentication and authorization services
            var authServicesToRemove = services
                .Where(descriptor => descriptor.ServiceType.Namespace != null &&
                                   (descriptor.ServiceType.Namespace.Contains("Authentication") ||
                                    descriptor.ServiceType.Namespace.Contains("Authorization") ||
                                    descriptor.ServiceType == typeof(IAuthenticationService) ||
                                    descriptor.ServiceType == typeof(IAuthorizationService)))
                .ToList();

            foreach (var service in authServicesToRemove)
            {
                services.Remove(service);
            }

            // Add fresh in-memory database context
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "FinanceTestDb");
                options.EnableSensitiveDataLogging();
            });

            // Re-register the required services  
            services.AddScoped<IRepositoryContext>(sp => sp.GetRequiredService<RepositoryContext>());
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            // Add test authentication that always succeeds
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });

            // Add test authorization that allows everything
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder("Test")
                    .RequireAssertion(_ => true)
                    .Build();
            });

            // Configure logging for tests
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            });
        });

        // Set environment variables for testing
        Environment.SetEnvironmentVariable("SECRET", "TEST_SECRET_KEY_FOR_INTEGRATION_TESTS_MINIMUM_32_CHARS");

        builder.UseEnvironment("Testing");
    }
}