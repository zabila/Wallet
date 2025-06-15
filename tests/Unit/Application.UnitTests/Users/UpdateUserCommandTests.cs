using Application.Users.Update;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Users;

public class UpdateUserCommandTests
{
    [Fact]
    public void UpdateUserCommand_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var firstName = "John";
        var lastName = "Doe";
        var telegramUserId = 123456789;
        var telegramUsername = "johndoe";
        var localization = "en-US";

        // Act
        var command = new UpdateUserCommand
        {
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            TelegramUserId = telegramUserId,
            TelegramUsername = telegramUsername,
            Localization = localization
        };

        // Assert
        command.UserId.Should().Be(userId);
        command.FirstName.Should().Be(firstName);
        command.LastName.Should().Be(lastName);
        command.TelegramUserId.Should().Be(telegramUserId);
        command.TelegramUsername.Should().Be(telegramUsername);
        command.Localization.Should().Be(localization);
    }

    [Fact]
    public void UpdateUserCommand_ShouldBeAssignableToICommand()
    {
        // Arrange & Act
        var command = new UpdateUserCommand();

        // Assert
        command.Should().BeAssignableTo<Application.Messaging.ICommand>();
    }

    [Fact]
    public void UpdateUserCommand_ShouldBeRecord()
    {
        // Arrange
        var command1 = new UpdateUserCommand
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            TelegramUserId = 123456789,
            TelegramUsername = "johndoe",
            Localization = "en-US"
        };

        var command2 = new UpdateUserCommand
        {
            UserId = command1.UserId,
            FirstName = command1.FirstName,
            LastName = command1.LastName,
            TelegramUserId = command1.TelegramUserId,
            TelegramUsername = command1.TelegramUsername,
            Localization = command1.Localization
        };

        // Assert - Records have value equality
        command1.Should().Be(command2);
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void UpdateUserCommand_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var command1 = new UpdateUserCommand
        {
            UserId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe"
        };

        var command2 = new UpdateUserCommand
        {
            UserId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith"
        };

        // Assert
        command1.Should().NotBe(command2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("John")]
    [InlineData("Very Long First Name")]
    public void UpdateUserCommand_FirstName_ShouldAcceptVariousValues(string firstName)
    {
        // Act
        var command = new UpdateUserCommand { FirstName = firstName };

        // Assert
        command.FirstName.Should().Be(firstName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Doe")]
    [InlineData("Very Long Last Name")]
    public void UpdateUserCommand_LastName_ShouldAcceptVariousValues(string lastName)
    {
        // Act
        var command = new UpdateUserCommand { LastName = lastName };

        // Assert
        command.LastName.Should().Be(lastName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123456789)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void UpdateUserCommand_TelegramUserId_ShouldAcceptVariousValues(int telegramUserId)
    {
        // Act
        var command = new UpdateUserCommand { TelegramUserId = telegramUserId };

        // Assert
        command.TelegramUserId.Should().Be(telegramUserId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("johndoe")]
    [InlineData("user_with_underscores")]
    [InlineData("UserWithCamelCase")]
    public void UpdateUserCommand_TelegramUsername_ShouldAcceptVariousValues(string telegramUsername)
    {
        // Act
        var command = new UpdateUserCommand { TelegramUsername = telegramUsername };

        // Assert
        command.TelegramUsername.Should().Be(telegramUsername);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("es-ES")]
    [InlineData("fr-FR")]
    [InlineData("")]
    public void UpdateUserCommand_Localization_ShouldAcceptVariousValues(string localization)
    {
        // Act
        var command = new UpdateUserCommand { Localization = localization };

        // Assert
        command.Localization.Should().Be(localization);
    }

    [Fact]
    public void UpdateUserCommand_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var command = new UpdateUserCommand();

        // Assert
        command.UserId.Should().Be(Guid.Empty);
        command.FirstName.Should().BeNull();
        command.LastName.Should().BeNull();
        command.TelegramUserId.Should().Be(0);
        command.TelegramUsername.Should().BeNull();
        command.Localization.Should().BeNull();
    }

    [Fact]
    public void UpdateUserCommand_ShouldGenerateCorrectHashCode()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command1 = new UpdateUserCommand
        {
            UserId = userId,
            FirstName = "John",
            LastName = "Doe"
        };

        var command2 = new UpdateUserCommand
        {
            UserId = userId,
            FirstName = "John",
            LastName = "Doe"
        };

        // Assert
        command1.GetHashCode().Should().Be(command2.GetHashCode());
    }
}