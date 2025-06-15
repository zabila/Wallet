using System.Globalization;
using System.Net;
using API.Finance.IntegrationTests.Common;
using Domain.Accounts;
using Domain.Transactions;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DTO.Transactions;
using TestUtilities.Builders;
using TestUtilities.Extensions;
using Xunit;
using LocationDto = SharedKernel.DTO.Transactions.Location;

namespace API.Finance.IntegrationTests.Controllers;

public sealed class TransactionControllerTests : IntegrationTestBase
{
    private readonly Guid _testUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public TransactionControllerTests(FinanceWebApplicationFactory factory) : base(factory)
    {
    }

    protected override async Task CreateTestUsersAsync(UserManager<User> userManager)
    {
        await base.CreateTestUsersAsync(userManager);

        // Ensure the transaction test user exists in the repository context
        var dbContext = GetDbContext();
        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == _testUserId);

        if (existingUser == null)
        {
            // Create the user directly in the repository context
            var user = new User
            {
                Id = _testUserId,
                Email = "transaction@finance.com",
                UserName = "transaction@finance.com",
                FirstName = "Transaction",
                LastName = "Tester",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Localization = "en-US",
                PasswordHash = "AQAAAAEAACcQAAAAEDummyPasswordHashForTestingPurposes",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Create an account for this user
            var account = AccountBuilder.Create()
                .WithAccountName("Test Transaction Account")
                .WithAccountType("Checking")
                .WithCurrency("USD")
                .WithBalance(1000.00m)
                .Build();

            dbContext.Accounts.Add(account);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            // Update user with account reference
            user.Account = account;
            user.AccountId = account.Id;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task CreateTransaction_WithValidRequest_ShouldReturnCreatedTransaction()
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = TestDataExtensions.GenerateRandomDecimal(1, 500),
            Category = "Food",
            Type = "Expense",
            Location = new LocationDto
            {
                Latitude = (double)TestDataExtensions.GenerateRandomDecimal(-90, 90),
                Longitude = (double)TestDataExtensions.GenerateRandomDecimal(-180, 180)
            },
            Attachment = "receipt.jpg"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionId = await DeserializeResponseAsync<Guid>(response);
        transactionId.Should().NotBeEmpty();

        // Verify transaction was created in database
        var dbContext = GetDbContext();
        var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        transaction.Should().NotBeNull();
        transaction!.Amount.Should().Be(request.Amount);
        transaction.Category.Should().Be(request.Category);
        transaction.Type.Should().Be(request.Type);
        transaction.UserId.Should().Be(_testUserId);
    }

    [Fact]
    public async Task CreateTransaction_WithMinimalData_ShouldSucceed()
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = 25.50m,
            Category = "Miscellaneous",
            Type = "Expense",
            Location = new LocationDto { Latitude = 0, Longitude = 0 },
            Attachment = ""
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionId = await DeserializeResponseAsync<Guid>(response);
        transactionId.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("Income")]
    [InlineData("Expense")]
    [InlineData("Transfer")]
    public async Task CreateTransaction_WithDifferentTypes_ShouldSucceed(string transactionType)
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = TestDataExtensions.GenerateRandomDecimal(10, 200),
            Category = "Test Category",
            Type = transactionType,
            Location = new LocationDto { Latitude = 40.7128, Longitude = -74.0060 },
            Attachment = "test.pdf"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, $"Failed for transaction type {transactionType}");

        var transactionId = await DeserializeResponseAsync<Guid>(response);
        transactionId.Should().NotBeEmpty();

        // Verify the transaction type was set correctly
        var dbContext = GetDbContext();
        var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        transaction!.Type.Should().Be(transactionType);
    }

    [Theory]
    [InlineData("Food")]
    [InlineData("Transportation")]
    [InlineData("Entertainment")]
    [InlineData("Healthcare")]
    [InlineData("Shopping")]
    [InlineData("Utilities")]
    public async Task CreateTransaction_WithDifferentCategories_ShouldSucceed(string category)
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = TestDataExtensions.GenerateRandomDecimal(5, 100),
            Category = category,
            Type = "Expense",
            Location = new LocationDto { Latitude = 37.7749, Longitude = -122.4194 },
            Attachment = $"{category.ToUpperInvariant()}_receipt.jpg"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, $"Failed for category {category}");

        var transactionId = await DeserializeResponseAsync<Guid>(response);
        transactionId.Should().NotBeEmpty();

        // Verify the category was set correctly
        var dbContext = GetDbContext();
        var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        transaction!.Category.Should().Be(category);
    }

    [Fact]
    public async Task CreateTransaction_WithLargeAmount_ShouldSucceed()
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = 99999.99m,
            Category = "Investment",
            Type = "Income",
            Location = new LocationDto { Latitude = 51.5074, Longitude = -0.1278 },
            Attachment = "investment_certificate.pdf"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTransaction_WithNegativeAmount_ShouldSucceed()
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = -50.00m,
            Category = "Refund",
            Type = "Income",
            Location = new LocationDto { Latitude = 35.6762, Longitude = 139.6503 },
            Attachment = "refund_receipt.jpg"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTransaction_ForNonExistentUser_ShouldReturnError()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();
        var request = new TransactionsRequest
        {
            Amount = 100,
            Category = "Test",
            Type = "Expense",
            Location = new LocationDto { Latitude = 0, Longitude = 0 },
            Attachment = ""
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{nonExistentUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTransactions_WithValidUserId_ShouldReturnTransactions()
    {
        // Arrange
        // First, create some transactions
        var transactions = new[]
        {
            new TransactionsRequest
            {
                Amount = 50.00m,
                Category = "Food",
                Type = "Expense",
                Location = new LocationDto { Latitude = 40.7128, Longitude = -74.0060 },
                Attachment = "lunch.jpg"
            },
            new TransactionsRequest
            {
                Amount = 1000.00m,
                Category = "Salary",
                Type = "Income",
                Location = new LocationDto { Latitude = 40.7128, Longitude = -74.0060 },
                Attachment = "payslip.pdf"
            }
        };

        foreach (var transaction in transactions)
        {
            using var content = CreateJsonContent(transaction);
            await HttpClient.PostAsync(
                $"/api/account/{_testUserId}/transactions/create",
                content);
        }

        // Act
        var response = await HttpClient.GetAsync($"/api/account/{_testUserId}/transactions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionsList = await DeserializeResponseAsync<List<TransactionsResponse>>(response);
        transactionsList.Should().NotBeNull();
        transactionsList!.Should().HaveCountGreaterOrEqualTo(2);

        // Verify transaction data
        if (transactionsList?.Count > 0)
        {
            var foodTransaction = transactionsList.FirstOrDefault(t => t.Category == "Food");
            foodTransaction.Should().NotBeNull();
            foodTransaction!.Amount.Should().Be(50.00m);
            foodTransaction.Type.Should().Be("Expense");
        }
    }

    [Fact]
    public async Task GetTransactions_ForUserWithNoTransactions_ShouldReturnError()
    {
        // Arrange - Create a user with no transactions
        var userManager = GetService<UserManager<User>>();
        var emptyUserId = Guid.NewGuid();
        var emptyUser = UserBuilder.Create()
            .WithId(emptyUserId)
            .WithEmail($"empty-{emptyUserId}@finance.com")
            .WithFirstName("Empty")
            .WithLastName("User")
            .Build();

        await userManager.CreateAsync(emptyUser, "TestPassword123!");

        // Create an account but no transactions
        var dbContext = GetDbContext();
        var account = AccountBuilder.Create()
            .WithAccountName("Empty Account")
            .WithAccountType("Checking")
            .WithCurrency("USD")
            .Build();

        dbContext.Accounts.Add(account);
        emptyUser.Account = account;
        emptyUser.AccountId = account.Id;
        dbContext.Users.Update(emptyUser);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await HttpClient.GetAsync($"/api/account/{emptyUserId}/transactions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTransactions_ForNonExistentUser_ShouldReturnError()
    {
        // Arrange
        var nonExistentUserId = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/account/{nonExistentUserId}/transactions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransaction_WithSpecialCharactersInCategory_ShouldSucceed()
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = 25.99m,
            Category = "Café & Restaurant",
            Type = "Expense",
            Location = new LocationDto { Latitude = 48.8566, Longitude = 2.3522 },
            Attachment = "café_receipt.jpg"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionId = await DeserializeResponseAsync<Guid>(response);
        transactionId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateTransaction_WithExtremCoordinates_ShouldSucceed()
    {
        // Arrange
        var request = new TransactionsRequest
        {
            Amount = 100.00m,
            Category = "Travel",
            Type = "Expense",
            Location = new LocationDto
            {
                Latitude = -89.9999,  // Near South Pole
                Longitude = 179.9999  // Near International Date Line
            },
            Attachment = "travel_receipt.jpg"
        };

        // Act
        using var content = CreateJsonContent(request);
        var response = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTransaction_ConcurrentRequests_ShouldHandleGracefully()
    {
        // Arrange
        var requests = Enumerable.Range(1, 10).Select(i => new TransactionsRequest
        {
            Amount = i * 10.5m,
            Category = $"Category {i}",
            Type = i % 2 == 0 ? "Income" : "Expense",
            Location = new LocationDto { Latitude = i, Longitude = i },
            Attachment = $"receipt_{i}.jpg"
        }).ToArray();

        // Act
        var tasks = requests.Select(request =>
        {
            var content = CreateJsonContent(request);
            return HttpClient.PostAsync(
                $"/api/account/{_testUserId}/transactions/create",
                content);
        }).ToArray();

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().AllSatisfy(response =>
            response.StatusCode.Should().Be(HttpStatusCode.OK));

        // Verify all transactions were created
        var dbContext = GetDbContext();
        var transactionCount = await dbContext.Transactions
            .CountAsync(t => t.UserId == _testUserId && t.Category.StartsWith("Category "));
        transactionCount.Should().Be(10);
    }

    [Fact]
    public async Task TransactionWorkflow_CreateAndRetrieve_ShouldWorkCorrectly()
    {
        // Arrange
        var createRequest = new TransactionsRequest
        {
            Amount = 75.50m,
            Category = "Workflow Test",
            Type = "Expense",
            Location = new LocationDto { Latitude = 52.5200, Longitude = 13.4050 },
            Attachment = "workflow_test.pdf"
        };

        // Act 1: Create transaction
        using var content = CreateJsonContent(createRequest);
        var createResponse = await HttpClient.PostAsync(
            $"/api/account/{_testUserId}/transactions/create",
            content);

        // Assert 1: Transaction created successfully
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var transactionId = await DeserializeResponseAsync<Guid>(createResponse);
        transactionId.Should().NotBeEmpty();

        // Act 2: Retrieve transactions
        var getResponse = await HttpClient.GetAsync($"/api/account/{_testUserId}/transactions");

        // Assert 2: Transaction appears in the list
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var transactions = await DeserializeResponseAsync<List<TransactionsResponse>>(getResponse);
        transactions.Should().NotBeNull();

        var createdTransaction = transactions!.FirstOrDefault(t => t.Id == transactionId);
        createdTransaction.Should().NotBeNull();
        createdTransaction!.Amount.Should().Be(75.50m);
        createdTransaction.Category.Should().Be("Workflow Test");
        createdTransaction.Type.Should().Be("Expense");
    }
}