using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Domain.Users;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TestUtilities.Fixtures;
using Xunit;

namespace API.Finance.IntegrationTests.Common;

public abstract class IntegrationTestBase : TestFixture, IClassFixture<FinanceWebApplicationFactory>, IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected readonly FinanceWebApplicationFactory Factory;
    protected readonly HttpClient HttpClient;
    private IServiceScope? _scope;

    protected IntegrationTestBase(FinanceWebApplicationFactory factory)
    {
        Factory = factory;
        HttpClient = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        _scope = Factory.Services.CreateScope();
        await ClearDatabaseAsync();
        await SeedTestDataAsync();

        // Set up authentication for all requests
        var token = GenerateJwtToken("test@finance.com");
        SetAuthorizationHeader(token);
    }

    public async Task DisposeAsync()
    {
        _scope?.Dispose();
        HttpClient.Dispose();
        await Task.CompletedTask;
    }

    protected virtual async Task SeedTestDataAsync()
    {
        var context = GetDbContext();
        var userManager = GetService<UserManager<User>>();

        await context.Database.EnsureCreatedAsync();

        // Create test users if they don't exist
        await CreateTestUsersAsync(userManager);

        // Ensure all changes are saved
        await context.SaveChangesAsync();
    }

    protected virtual async Task CreateTestUsersAsync(UserManager<User> userManager)
    {
        var testUser = await userManager.FindByEmailAsync("test@finance.com");
        if (testUser == null)
        {
            var user = new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "test@finance.com",
                UserName = "test@finance.com",
                FirstName = "Test",
                LastName = "User",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Localization = "en-US"
            };

            var result = await userManager.CreateAsync(user, "TestPassword123!");
            if (!result.Succeeded)
            {
                // Log the errors but don't throw - the user might already exist
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"User creation error: {error.Description}");
                }
            }
        }
    }

    protected string GenerateJwtToken(string email)
    {
        var configuration = GetService<IConfiguration>();
        var secret = Environment.GetEnvironmentVariable("SECRET") ?? "TEST_SECRET_KEY_FOR_INTEGRATION_TESTS_MINIMUM_32_CHARS";
        var key = Encoding.UTF8.GetBytes(secret);
        var secretKey = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("user_name", email),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111")
        };

        var tokenOptions = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"] ?? "TestIssuer",
            audience: configuration["Jwt:Audience"] ?? "TestAudience",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    protected void SetAuthorizationHeader(string token)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected static StringContent CreateJsonContent<T>(T content)
    {
        var json = JsonSerializer.Serialize(content, JsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected static async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    protected RepositoryContext GetDbContext()
    {
        return _scope?.ServiceProvider.GetRequiredService<RepositoryContext>()
               ?? throw new InvalidOperationException("Database context is not available");
    }

    protected T GetService<T>() where T : class
    {
        return _scope?.ServiceProvider.GetRequiredService<T>()
               ?? throw new InvalidOperationException($"Service {typeof(T).Name} is not available");
    }

    private async Task ClearDatabaseAsync()
    {
        var context = GetDbContext();

        context.Users.RemoveRange(context.Users);
        context.Accounts.RemoveRange(context.Accounts);
        context.Transactions.RemoveRange(context.Transactions);

        await context.SaveChangesAsync();
    }
}
