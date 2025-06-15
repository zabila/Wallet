using Domain.Users;
using Domain.Accounts;
using Domain.Transactions;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeCollections()
    {
        // Act
        var user = new User();

        // Assert
        user.Transactions.Should().NotBeNull();
        user.Transactions.Should().BeEmpty();
    }

    [Fact]
    public void User_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddMinutes(5);

        // Act
        var user = new User
        {
            Id = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            UserName = "john.doe@example.com",
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            Localization = "en-US",
            TelegramId = telegramId,
            AccountId = accountId
        };

        // Assert
        user.Id.Should().Be(userId);
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        user.Email.Should().Be("john.doe@example.com");
        user.UserName.Should().Be("john.doe@example.com");
        user.CreatedAt.Should().Be(createdAt);
        user.UpdatedAt.Should().Be(updatedAt);
        user.Localization.Should().Be("en-US");
        user.TelegramId.Should().Be(telegramId);
        user.AccountId.Should().Be(accountId);
    }

    [Fact]
    public void User_TelegramUser_ShouldAllowNullValue()
    {
        // Arrange & Act
        var user = new User
        {
            TelegramUser = null,
            TelegramId = null
        };

        // Assert
        user.TelegramUser.Should().BeNull();
        user.TelegramId.Should().BeNull();
    }

    [Fact]
    public void User_Account_ShouldAllowNullValue()
    {
        // Arrange & Act
        var user = new User
        {
            Account = null,
            AccountId = null
        };

        // Assert
        user.Account.Should().BeNull();
        user.AccountId.Should().BeNull();
    }

    [Fact]
    public void User_ShouldInheritFromIdentityUser()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Should().BeAssignableTo<Microsoft.AspNetCore.Identity.IdentityUser<Guid>>();
    }

    [Fact]
    public void User_WithTelegramUser_ShouldSetRelationshipCorrectly()
    {
        // Arrange
        var user = new User();
        var telegramUser = new TelegramUser
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 123456789,
            TelegramUsername = "johndoe",
            UserId = user.Id
        };

        // Act
        user.TelegramUser = telegramUser;
        user.TelegramId = telegramUser.Id;

        // Assert
        user.TelegramUser.Should().Be(telegramUser);
        user.TelegramId.Should().Be(telegramUser.Id);
    }

    [Fact]
    public void User_WithAccount_ShouldSetRelationshipCorrectly()
    {
        // Arrange
        var user = new User();
        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountName = "John's Account",
            AccountType = "Personal",
            Balance = 1000.00m,
            Currency = "USD"
        };

        // Act
        user.Account = account;
        user.AccountId = account.Id;

        // Assert
        user.Account.Should().Be(account);
        user.AccountId.Should().Be(account.Id);
    }

    [Fact]
    public void User_WithTransactions_ShouldManageCollectionCorrectly()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var transaction1 = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 100.00m,
            Category = "Food",
            Type = "Expense",
            UserId = user.Id
        };
        var transaction2 = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 50.00m,
            Category = "Transport",
            Type = "Expense",
            UserId = user.Id
        };

        // Act
        user.Transactions.Add(transaction1);
        user.Transactions.Add(transaction2);

        // Assert
        user.Transactions.Should().HaveCount(2);
        user.Transactions.Should().Contain(transaction1);
        user.Transactions.Should().Contain(transaction2);
    }
}