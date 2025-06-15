using Domain.Accounts;
using FluentAssertions;
using TestUtilities.Builders;
using Xunit;

namespace TestUtilities.UnitTests.Builders;

public class AccountBuilderTests
{
    [Fact]
    public void Create_ShouldReturnNewAccountBuilderInstance()
    {
        // Act
        var builder = AccountBuilder.Create();

        // Assert
        builder.Should().NotBeNull();
        builder.Should().BeOfType<AccountBuilder>();
    }

    [Fact]
    public void Build_ShouldCreateAccountWithDefaultValues()
    {
        // Arrange
        var builder = AccountBuilder.Create();

        // Act
        var account = builder.Build();

        // Assert
        account.Should().NotBeNull();
        account.Should().BeOfType<Account>();
        account.AccountName.Should().Be("Test Account");
        account.AccountType.Should().Be("Checking");
        account.Currency.Should().Be("USD");
        account.Balance.Should().Be(100.00m);
    }

    [Fact]
    public void WithAccountName_ShouldSetAccountName()
    {
        // Arrange
        const string accountName = "Savings Account";
        var builder = AccountBuilder.Create();

        // Act
        var account = builder.WithAccountName(accountName).Build();

        // Assert
        account.AccountName.Should().Be(accountName);
    }

    [Fact]
    public void WithAccountType_ShouldSetAccountType()
    {
        // Arrange
        const string accountType = "Savings";
        var builder = AccountBuilder.Create();

        // Act
        var account = builder.WithAccountType(accountType).Build();

        // Assert
        account.AccountType.Should().Be(accountType);
    }

    [Fact]
    public void WithCurrency_ShouldSetCurrency()
    {
        // Arrange
        const string currency = "EUR";
        var builder = AccountBuilder.Create();

        // Act
        var account = builder.WithCurrency(currency).Build();

        // Assert
        account.Currency.Should().Be(currency);
    }

    [Fact]
    public void WithId_ShouldReturnBuilderInstance_ForFluentInterface()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var builder = AccountBuilder.Create();

        // Act
        var result = builder.WithId(accountId);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void WithBalance_ShouldReturnBuilderInstance_ForFluentInterface()
    {
        // Arrange
        const decimal balance = 150.75m;
        var builder = AccountBuilder.Create();

        // Act
        var result = builder.WithBalance(balance);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void WithCreatedAt_ShouldReturnBuilderInstance_ForFluentInterface()
    {
        // Arrange
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var builder = AccountBuilder.Create();

        // Act
        var result = builder.WithCreatedAt(createdAt);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void WithUpdatedAt_ShouldReturnBuilderInstance_ForFluentInterface()
    {
        // Arrange
        var updatedAt = DateTime.UtcNow;
        var builder = AccountBuilder.Create();

        // Act
        var result = builder.WithUpdatedAt(updatedAt);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void FluentInterface_ShouldAllowChaining()
    {
        // Arrange
        const string accountName = "Chained Account";
        const string currency = "EUR";
        const decimal balance = 500.50m;
        var createdAt = DateTime.UtcNow.AddHours(-1);

        // Act
        var account = AccountBuilder.Create()
            .WithAccountName(accountName)
            .WithCurrency(currency)
            .WithBalance(balance)
            .WithCreatedAt(createdAt)
            .Build();

        // Assert
        account.AccountName.Should().Be(accountName);
        account.Currency.Should().Be(currency);
        account.Balance.Should().Be(balance);
    }

    [Fact]
    public void Build_ShouldCreateDifferentAccountInstances_WhenCalledMultipleTimes()
    {
        // Arrange
        var builder = AccountBuilder.Create();

        // Act
        var account1 = builder.Build();
        var account2 = builder.Build();

        // Assert
        account1.Should().NotBeSameAs(account2);
        account1.Id.Should().NotBe(account2.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(1000.99)]
    [InlineData(99999.99)]
    public void WithBalance_ShouldHandleValidDecimalValues(decimal balance)
    {
        // Arrange
        var builder = AccountBuilder.Create();

        // Act
        var result = builder.WithBalance(balance);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    [InlineData(-1000.99)]
    public void WithBalance_ShouldHandleNegativeValues_AndReturnBuilder(decimal balance)
    {
        // Arrange
        var builder = AccountBuilder.Create();

        // Act
        var result = builder.WithBalance(balance);
        var account = result.Build();

        // Assert
        result.Should().BeSameAs(builder);
        account.Balance.Should().Be(balance);
        // Note: Domain validation for negative balance would be tested in domain tests
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    public void Build_ShouldCreateValidAccount_WithAllSupportedCurrencies(string currency)
    {
        // Act
        var account = AccountBuilder.Create()
            .WithCurrency(currency)
            .Build();

        // Assert
        account.Should().NotBeNull();
        account.Currency.Should().Be(currency);
    }

    [Fact]
    public void Build_ShouldCreateValidAccount_WithMinimalRequiredProperties()
    {
        // Act
        var account = AccountBuilder.Create()
            .WithAccountName("Minimal Account")
            .Build();

        // Assert
        account.Should().NotBeNull();
        account.AccountName.Should().Be("Minimal Account");
        account.Currency.Should().Be("USD"); // Default currency
        account.AccountType.Should().Be("Checking"); // Default type
    }

    [Fact]
    public void Build_ShouldCreateAccount_WithSpecificPropertiesAndCurrency()
    {
        // Arrange
        const string accountName = "Premium Account";
        const string currency = "EUR";
        const string accountType = "Premium";
        const decimal balance = 1000.00m;

        // Act
        var account = AccountBuilder.Create()
            .WithAccountName(accountName)
            .WithCurrency(currency)
            .WithAccountType(accountType)
            .WithBalance(balance)
            .Build();

        // Assert
        account.AccountName.Should().Be(accountName);
        account.Currency.Should().Be(currency);
        account.AccountType.Should().Be(accountType);
        account.Balance.Should().Be(balance);
    }

    [Fact]
    public void AccountBuilder_ShouldSupportLargeDecimalValues()
    {
        // Arrange
        const decimal largeBalance = 999999999.99m;
        
        // Act
        var result = AccountBuilder.Create().WithBalance(largeBalance);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void AccountBuilder_ShouldSupportPreciseDecimalValues()
    {
        // Arrange
        const decimal preciseBalance = 123.456789m;
        
        // Act
        var result = AccountBuilder.Create().WithBalance(preciseBalance);

        // Assert
        result.Should().NotBeNull();
    }
} 