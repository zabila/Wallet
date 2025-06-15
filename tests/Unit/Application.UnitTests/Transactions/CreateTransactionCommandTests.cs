using Application.Transactions.Create;
using Domain.Transactions;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Transactions;

public class CreateTransactionCommandTests
{
    [Fact]
    public void CreateTransactionCommand_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var amount = 100.50m;
        var category = "Food";
        var type = "Expense";
        var location = new Location { Latitude = 40.7128, Longitude = -74.0060 };
        var attachment = "receipt.jpg";

        // Act
        var command = new CreateTransactionCommand
        {
            UserId = userId,
            Amount = amount,
            Category = category,
            Type = type,
            Location = location,
            Attachment = attachment
        };

        // Assert
        command.UserId.Should().Be(userId);
        command.Amount.Should().Be(amount);
        command.Category.Should().Be(category);
        command.Type.Should().Be(type);
        command.Location.Should().Be(location);
        command.Attachment.Should().Be(attachment);
    }

    [Fact]
    public void CreateTransactionCommand_ShouldBeAssignableToICommandOfGuid()
    {
        // Arrange & Act
        var command = new CreateTransactionCommand();

        // Assert
        command.Should().BeAssignableTo<Application.Messaging.ICommand<Guid>>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50.25)]
    [InlineData(-25.75)]
    [InlineData(9999.99)]
    [InlineData(0.01)]
    public void CreateTransactionCommand_Amount_ShouldAcceptVariousValues(decimal amount)
    {
        // Act
        var command = new CreateTransactionCommand { Amount = amount };

        // Assert
        command.Amount.Should().Be(amount);
    }

    [Theory]
    [InlineData("Food")]
    [InlineData("Transportation")]
    [InlineData("Entertainment")]
    [InlineData("Utilities")]
    [InlineData("")]
    public void CreateTransactionCommand_Category_ShouldAcceptVariousValues(string category)
    {
        // Act
        var command = new CreateTransactionCommand { Category = category };

        // Assert
        command.Category.Should().Be(category);
    }

    [Theory]
    [InlineData("Income")]
    [InlineData("Expense")]
    [InlineData("Transfer")]
    [InlineData("Refund")]
    [InlineData("")]
    public void CreateTransactionCommand_Type_ShouldAcceptVariousValues(string type)
    {
        // Act
        var command = new CreateTransactionCommand { Type = type };

        // Assert
        command.Type.Should().Be(type);
    }

    [Theory]
    [InlineData("receipt.jpg")]
    [InlineData("invoice.pdf")]
    [InlineData("photo.png")]
    [InlineData("")]
    public void CreateTransactionCommand_Attachment_ShouldAcceptVariousValues(string attachment)
    {
        // Act
        var command = new CreateTransactionCommand { Attachment = attachment };

        // Assert
        command.Attachment.Should().Be(attachment);
    }

    [Fact]
    public void CreateTransactionCommand_Location_ShouldAcceptLocationObject()
    {
        // Arrange
        var location = new Location
        {
            Latitude = 40.7128,
            Longitude = -74.0060
        };

        // Act
        var command = new CreateTransactionCommand { Location = location };

        // Assert
        command.Location.Should().Be(location);
        command.Location.Latitude.Should().Be(40.7128);
        command.Location.Longitude.Should().Be(-74.0060);
    }

    [Fact]
    public void CreateTransactionCommand_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var command = new CreateTransactionCommand();

        // Assert
        command.UserId.Should().Be(Guid.Empty);
        command.Amount.Should().Be(0);
        command.Category.Should().BeNull();
        command.Type.Should().BeNull();
        command.Location.Should().BeNull();
        command.Attachment.Should().BeNull();
    }

    [Fact]
    public void CreateTransactionCommand_WithNegativeAmount_ShouldAcceptValue()
    {
        // Arrange & Act
        var command = new CreateTransactionCommand { Amount = -100.50m };

        // Assert
        command.Amount.Should().Be(-100.50m);
    }

    [Fact]
    public void CreateTransactionCommand_WithZeroAmount_ShouldAcceptValue()
    {
        // Arrange & Act
        var command = new CreateTransactionCommand { Amount = 0m };

        // Assert
        command.Amount.Should().Be(0m);
    }

    [Fact]
    public void CreateTransactionCommand_WithLargeAmount_ShouldAcceptValue()
    {
        // Arrange
        var largeAmount = decimal.MaxValue;

        // Act
        var command = new CreateTransactionCommand { Amount = largeAmount };

        // Assert
        command.Amount.Should().Be(largeAmount);
    }

    [Fact]
    public void CreateTransactionCommand_WithCompleteValidData_ShouldCreateCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var location = new Location { Latitude = 51.5074, Longitude = -0.1278 }; // London coordinates

        // Act
        var command = new CreateTransactionCommand
        {
            UserId = userId,
            Amount = 75.99m,
            Category = "Groceries",
            Type = "Expense",
            Location = location,
            Attachment = "receipt_20240205.jpg"
        };

        // Assert
        command.UserId.Should().Be(userId);
        command.Amount.Should().Be(75.99m);
        command.Category.Should().Be("Groceries");
        command.Type.Should().Be("Expense");
        command.Location.Should().Be(location);
        command.Location.Latitude.Should().Be(51.5074);
        command.Location.Longitude.Should().Be(-0.1278);
        command.Attachment.Should().Be("receipt_20240205.jpg");
    }

    [Theory]
    [InlineData(90.0, 0.0)]        // North Pole
    [InlineData(-90.0, 0.0)]       // South Pole  
    [InlineData(40.7128, -74.0060)] // New York
    [InlineData(35.6762, 139.6503)] // Tokyo
    [InlineData(-33.8688, 151.2093)] // Sydney
    public void CreateTransactionCommand_Location_ShouldAcceptValidCoordinates(double latitude, double longitude)
    {
        // Arrange
        var location = new Location { Latitude = latitude, Longitude = longitude };

        // Act
        var command = new CreateTransactionCommand { Location = location };

        // Assert
        command.Location.Latitude.Should().Be(latitude);
        command.Location.Longitude.Should().Be(longitude);
    }

    [Fact]
    public void CreateTransactionCommand_UserId_ShouldAcceptValidGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var command = new CreateTransactionCommand { UserId = userId };

        // Assert
        command.UserId.Should().Be(userId);
    }

    [Fact]
    public void CreateTransactionCommand_WithNullLocation_ShouldAcceptValue()
    {
        // Arrange & Act
        var command = new CreateTransactionCommand { Location = null! };

        // Assert
        command.Location.Should().BeNull();
    }

    [Fact]
    public void CreateTransactionCommand_WithEmptyStrings_ShouldAcceptValues()
    {
        // Arrange & Act
        var command = new CreateTransactionCommand
        {
            Category = "",
            Type = "",
            Attachment = ""
        };

        // Assert
        command.Category.Should().Be("");
        command.Type.Should().Be("");
        command.Attachment.Should().Be("");
    }
}