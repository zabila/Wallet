using Application.Accounts.Create;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Accounts;

public class CreateAccountCommandTests
{
    [Fact]
    public void CreateAccountCommand_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountName = "Main Account";
        var accountType = "Savings";
        var balance = 1000.50m;
        var currency = "USD";

        // Act
        var command = new CreateAccountCommand
        {
            UserId = userId,
            AccountName = accountName,
            AccountType = accountType,
            Balance = balance,
            Currency = currency
        };

        // Assert
        command.UserId.Should().Be(userId);
        command.AccountName.Should().Be(accountName);
        command.AccountType.Should().Be(accountType);
        command.Balance.Should().Be(balance);
        command.Currency.Should().Be(currency);
    }

    [Fact]
    public void CreateAccountCommand_ShouldBeAssignableToICommandOfGuid()
    {
        // Arrange & Act
        var command = new CreateAccountCommand();

        // Assert
        command.Should().BeAssignableTo<Application.Messaging.ICommand<Guid>>();
    }

    [Theory]
    [InlineData("Main Account")]
    [InlineData("Savings Account")]
    [InlineData("Emergency Fund")]
    [InlineData("")]
    public void CreateAccountCommand_AccountName_ShouldAcceptVariousValues(string accountName)
    {
        // Act
        var command = new CreateAccountCommand { AccountName = accountName };

        // Assert
        command.AccountName.Should().Be(accountName);
    }

    [Theory]
    [InlineData("Savings")]
    [InlineData("Checking")]
    [InlineData("Credit")]
    [InlineData("Investment")]
    [InlineData("")]
    public void CreateAccountCommand_AccountType_ShouldAcceptVariousValues(string accountType)
    {
        // Act
        var command = new CreateAccountCommand { AccountType = accountType };

        // Assert
        command.AccountType.Should().Be(accountType);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100.50)]
    [InlineData(-50.25)]
    [InlineData(9999999.99)]
    public void CreateAccountCommand_Balance_ShouldAcceptVariousValues(decimal balance)
    {
        // Act
        var command = new CreateAccountCommand { Balance = balance };

        // Assert
        command.Balance.Should().Be(balance);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    [InlineData("CAD")]
    [InlineData("")]
    public void CreateAccountCommand_Currency_ShouldAcceptVariousValues(string currency)
    {
        // Act
        var command = new CreateAccountCommand { Currency = currency };

        // Assert
        command.Currency.Should().Be(currency);
    }

    [Fact]
    public void CreateAccountCommand_UserId_ShouldAcceptGuidValues()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var command = new CreateAccountCommand { UserId = userId };

        // Assert
        command.UserId.Should().Be(userId);
    }

    [Fact]
    public void CreateAccountCommand_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var command = new CreateAccountCommand();

        // Assert
        command.UserId.Should().Be(Guid.Empty);
        command.AccountName.Should().BeNull();
        command.AccountType.Should().BeNull();
        command.Balance.Should().Be(0);
        command.Currency.Should().BeNull();
    }

    [Fact]
    public void CreateAccountCommand_WithCompleteData_ShouldCreateCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var command = new CreateAccountCommand
        {
            UserId = userId,
            AccountName = "Business Account",
            AccountType = "Business Checking",
            Balance = 5000.00m,
            Currency = "USD"
        };

        // Assert
        command.UserId.Should().Be(userId);
        command.AccountName.Should().Be("Business Account");
        command.AccountType.Should().Be("Business Checking");
        command.Balance.Should().Be(5000.00m);
        command.Currency.Should().Be("USD");
    }

    [Fact]
    public void CreateAccountCommand_WithNegativeBalance_ShouldAcceptValue()
    {
        // Arrange & Act
        var command = new CreateAccountCommand { Balance = -100.50m };

        // Assert
        command.Balance.Should().Be(-100.50m);
    }

    [Fact]
    public void CreateAccountCommand_WithZeroBalance_ShouldAcceptValue()
    {
        // Arrange & Act
        var command = new CreateAccountCommand { Balance = 0m };

        // Assert
        command.Balance.Should().Be(0m);
    }

    [Fact]
    public void CreateAccountCommand_WithLargeBalance_ShouldAcceptValue()
    {
        // Arrange
        var largeBalance = decimal.MaxValue;

        // Act
        var command = new CreateAccountCommand { Balance = largeBalance };

        // Assert
        command.Balance.Should().Be(largeBalance);
    }

    [Fact]
    public void CreateAccountCommand_WithEmptyGuid_ShouldAcceptValue()
    {
        // Arrange & Act
        var command = new CreateAccountCommand { UserId = Guid.Empty };

        // Assert
        command.UserId.Should().Be(Guid.Empty);
    }
}