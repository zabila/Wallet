using System.Net;
using API.Finance.IntegrationTests.Common;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DTO.Accounts;
using TestUtilities.Builders;
using TestUtilities.Extensions;
using Xunit;

namespace API.Finance.IntegrationTests.Controllers;

public sealed class AccountControllerTests : IntegrationTestBase
{
    private readonly Guid _testUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public AccountControllerTests(FinanceWebApplicationFactory factory) : base(factory)
    {
    }

    protected override async Task CreateTestUsersAsync(UserManager<User> userManager)
    {
        await base.CreateTestUsersAsync(userManager);

        // Ensure the test user exists in the repository context
        var dbContext = GetDbContext();
        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == _testUserId);

        if (existingUser == null)
        {
            // Create the user directly in the repository context
            var user = new User
            {
                Id = _testUserId,
                Email = "account@finance.com",
                UserName = "account@finance.com",
                FirstName = "Account",
                LastName = "Tester",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Localization = "en-US",
                PasswordHash = "AQAAAAEAACcQAAAAEDummyPasswordHashForTestingPurposes",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task CreateAccount_WithValidRequest_ShouldReturnCreatedAccount()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = TestDataExtensions.GenerateRandomName(),
            AccountType = "Checking",
            Balance = TestDataExtensions.GenerateRandomDecimal(100, 1000),
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error Response: {response.StatusCode}");
            Console.WriteLine($"Error Content: {errorContent}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var accountId = await DeserializeResponseAsync<Guid>(response);
        accountId.Should().NotBeEmpty();

        // Verify account was created in database
        var dbContext = GetDbContext();
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        account.Should().NotBeNull();
        account!.AccountName.Should().Be(request.AccountName);
        account.AccountType.Should().Be(request.AccountType);
        account.Currency.Should().Be(request.Currency);
    }

    [Fact]
    public async Task CreateAccount_WithMinimalValidData_ShouldSucceed()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = "Minimal Account",
            AccountType = "Savings",
            Balance = 0,
            Currency = "EUR"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var accountId = await DeserializeResponseAsync<Guid>(response);
        accountId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAccount_WithLargeBalance_ShouldSucceed()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = "High Balance Account",
            AccountType = "Investment",
            Balance = 999999.99m,
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateAccount_WithDifferentCurrencies_ShouldSucceed()
    {
        // Arrange
        var currencies = new[] { "USD", "EUR", "GBP", "JPY" };

        foreach (var currency in currencies)
        {
            var userManager = GetService<UserManager<User>>();
            var uniqueUserId = Guid.NewGuid();
            var testUser = UserBuilder.Create()
                .WithId(uniqueUserId)
                .WithEmail($"test-{currency.ToUpperInvariant()}-{uniqueUserId}@example.com")
                .WithFirstName("Test")
                .WithLastName($"User_{currency}")
                .Build();
            await userManager.CreateAsync(testUser, "Password123!");

            var request = new AccountCreateRequest
            {
                AccountName = $"Account {currency}",
                AccountType = "Checking",
                Balance = 100,
                Currency = currency
            };

            // Act
            using var content = CreateJsonContent(request);
            var response = await HttpClient.PostAsync(
                $"/api/user/{uniqueUserId}/account/create",
                content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Currency: {currency}, Status: {response.StatusCode}, Error: {errorContent}");
            }

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Failed for currency {currency}");
        }
    }

    [Fact]
    public async Task CreateAccount_WithNegativeBalance_ShouldSucceed()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = "Overdraft Account",
            AccountType = "Checking",
            Balance = -100.50m,
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateAccount_ForNonExistentUser_ShouldReturnError()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();
        var request = new AccountCreateRequest
        {
            AccountName = "Test Account",
            AccountType = "Checking",
            Balance = 100,
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{nonExistentUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAccount_WithEmptyAccountName_ShouldReturnError()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = "",
            AccountType = "Checking",
            Balance = 100,
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAccount_WithEmptyAccountType_ShouldReturnError()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = "Test Account",
            AccountType = "",
            Balance = 100,
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAccount_WithEmptyCurrency_ShouldReturnError()
    {
        // Arrange
        var request = new AccountCreateRequest
        {
            AccountName = "Test Account",
            AccountType = "Checking",
            Balance = 100,
            Currency = ""
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAccount_WithVeryLongAccountName_ShouldHandleGracefully()
    {
        // Arrange
        var longName = TestDataExtensions.GenerateRandomString(500);
        var request = new AccountCreateRequest
        {
            AccountName = longName,
            AccountType = "Checking",
            Balance = 100,
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateAccount_WhenUserAlreadyHasAccount_ShouldReturnConflict()
    {
        // Arrange
        // First, create an account for the user
        var dbContext = GetDbContext();
        var user = await dbContext.Users.FirstAsync(u => u.Id == _testUserId);

        var existingAccount = AccountBuilder.Create()
            .WithAccountName("Existing Account")
            .WithAccountType("Checking")
            .WithCurrency("USD")
            .Build();

        dbContext.Accounts.Add(existingAccount);
        user.Account = existingAccount;
        user.AccountId = existingAccount.Id;
        await dbContext.SaveChangesAsync();

        var request = new AccountCreateRequest
        {
            AccountName = "Second Account",
            AccountType = "Savings",
            Balance = 200,
            Currency = "EUR"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{_testUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("Checking")]
    [InlineData("Savings")]
    [InlineData("Investment")]
    [InlineData("Credit")]
    public async Task CreateAccount_WithDifferentAccountTypes_ShouldSucceed(string accountType)
    {
        // Arrange
        var uniqueUserId = Guid.NewGuid();

        // Create a unique user for this test
        var userManager = GetService<UserManager<User>>();
        var testUser = UserBuilder.Create()
            .WithId(uniqueUserId)
            .WithEmail($"test-{uniqueUserId}@example.com")
            .WithFirstName("Test")
            .WithLastName("User")
            .Build();

        await userManager.CreateAsync(testUser, "Password123!");

        var request = new AccountCreateRequest
        {
            AccountName = $"{accountType} Account",
            AccountType = accountType,
            Balance = TestDataExtensions.GenerateRandomDecimal(0, 1000),
            Currency = "USD"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/user/{uniqueUserId}/account/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var accountId = await DeserializeResponseAsync<Guid>(response);
        accountId.Should().NotBeEmpty();

        // Verify the account type was set correctly
        var dbContext = GetDbContext();
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
        account!.AccountType.Should().Be(accountType);
    }

    [Fact]
    public async Task CreateAccount_ConcurrentRequests_ShouldHandleGracefully()
    {
        // Arrange
        var requests = Enumerable.Range(1, 5).Select(i => new AccountCreateRequest
        {
            AccountName = $"Concurrent Account {i}",
            AccountType = "Checking",
            Balance = i * 100,
            Currency = "USD"
        }).ToArray();

        // Create unique users for concurrent testing
        var userManager = GetService<UserManager<User>>();
        var userIds = new List<Guid>();

        for (int i = 0; i < 5; i++)
        {
            var userId = Guid.NewGuid();
            var testUser = UserBuilder.Create()
                .WithId(userId)
                .WithEmail($"concurrent-{userId}@example.com")
                .WithFirstName("Test")
                .WithLastName($"User{i}")
                .Build();

            await userManager.CreateAsync(testUser, "Password123!");
            userIds.Add(userId);
        }

        // Act
        var tasks = requests.Select((request, index) =>
        {
            var content = CreateJsonContent(request);
            return HttpClient.PostAsync(
                $"/api/user/{userIds[index]}/account/create",
                content);
        }).ToArray();

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().AllSatisfy(response =>
            response.StatusCode.Should().Be(HttpStatusCode.OK));
    }
}
