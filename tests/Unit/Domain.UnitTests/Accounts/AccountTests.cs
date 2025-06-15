using Domain.Accounts;
using Domain.Users;
using Domain.Transactions;
using FluentAssertions;
using SharedKernel;
using Xunit;

namespace Domain.UnitTests.Accounts;

public class AccountTests
{
    [Fact]
    public void Account_Constructor_ShouldInitializeCollections()
    {
        // Act
        var account = new Account();

        // Assert
        account.Transactions.Should().NotBeNull();
        account.Transactions.Should().BeEmpty();
        account.Users.Should().NotBeNull();
        account.Users.Should().BeEmpty();
    }

    [Fact]
    public void Account_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accountName = "John's Checking Account";
        var accountType = "Checking";
        var balance = 1500.75m;
        var currency = "USD";
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddMinutes(10);

        // Act
        var account = new Account
        {
            Id = accountId,
            AccountName = accountName,
            AccountType = accountType,
            Balance = balance,
            Currency = currency,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

        // Assert
        account.Id.Should().Be(accountId);
        account.AccountName.Should().Be(accountName);
        account.AccountType.Should().Be(accountType);
        account.Balance.Should().Be(balance);
        account.Currency.Should().Be(currency);
        account.CreatedAt.Should().Be(createdAt);
        account.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public void Account_ShouldInheritFromEntity()
    {
        // Arrange & Act
        var account = new Account();

        // Assert
        account.Should().BeAssignableTo<Entity>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100.50)]
    [InlineData(-50.25)]
    [InlineData(999999.99)]
    public void Account_Balance_ShouldAcceptValidDecimalValues(decimal balance)
    {
        // Arrange & Act
        var account = new Account
        {
            Balance = balance
        };

        // Assert
        account.Balance.Should().Be(balance);
    }

    [Theory]
    [InlineData("Checking")]
    [InlineData("Savings")]
    [InlineData("Investment")]
    [InlineData("Credit")]
    public void Account_AccountType_ShouldAcceptValidValues(string accountType)
    {
        // Arrange & Act
        var account = new Account
        {
            AccountType = accountType
        };

        // Assert
        account.AccountType.Should().Be(accountType);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    public void Account_Currency_ShouldAcceptValidValues(string currency)
    {
        // Arrange & Act
        var account = new Account
        {
            Currency = currency
        };

        // Assert
        account.Currency.Should().Be(currency);
    }

    [Fact]
    public void Account_WithTransactions_ShouldManageCollectionCorrectly()
    {
        // Arrange
        var account = new Account { Id = Guid.NewGuid() };
        var transaction1 = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 100.00m,
            Category = "Food",
            Type = "Expense",
            AccountId = account.Id
        };
        var transaction2 = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 200.00m,
            Category = "Salary",
            Type = "Income",
            AccountId = account.Id
        };

        // Act
        account.Transactions.Add(transaction1);
        account.Transactions.Add(transaction2);

        // Assert
        account.Transactions.Should().HaveCount(2);
        account.Transactions.Should().Contain(transaction1);
        account.Transactions.Should().Contain(transaction2);
    }

    [Fact]
    public void Account_WithUsers_ShouldManageCollectionCorrectly()
    {
        // Arrange
        var account = new Account { Id = Guid.NewGuid() };
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            AccountId = account.Id
        };
        var user2 = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            AccountId = account.Id
        };

        // Act
        account.Users.Add(user1);
        account.Users.Add(user2);

        // Assert
        account.Users.Should().HaveCount(2);
        account.Users.Should().Contain(user1);
        account.Users.Should().Contain(user2);
    }

    [Fact]
    public void Account_ShouldAllowEmptyCollections()
    {
        // Arrange & Act
        var account = new Account();

        // Assert
        account.Transactions.Should().BeEmpty();
        account.Users.Should().BeEmpty();
    }

    [Fact]
    public void Account_ShouldAllowMultipleTransactionTypes()
    {
        // Arrange
        var account = new Account { Id = Guid.NewGuid() };
        var incomeTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 1000.00m,
            Category = "Salary",
            Type = "Income",
            AccountId = account.Id
        };
        var expenseTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 50.00m,
            Category = "Food",
            Type = "Expense",
            AccountId = account.Id
        };

        // Act
        account.Transactions.Add(incomeTransaction);
        account.Transactions.Add(expenseTransaction);

        // Assert
        account.Transactions.Should().HaveCount(2);
        account.Transactions.Should().Contain(t => t.Type == "Income");
        account.Transactions.Should().Contain(t => t.Type == "Expense");
    }
}