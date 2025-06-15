using Application.Users.Register;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Users.Register;

public class RegisterUserCommandTests
{
    [Fact]
    public void RegisterUserCommand_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var email = "john.doe@example.com";
        var password = "SecurePassword123!";
        var firstName = "John";
        var lastName = "Doe";
        var telegramUserId = 123456789L;
        var telegramUsername = "johndoe";
        var localization = "en-US";

        // Act
        var command = new RegisterUserCommand
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            TelegramUserId = telegramUserId,
            TelegramUsername = telegramUsername,
            Localization = localization
        };

        // Assert
        command.Email.Should().Be(email);
        command.Password.Should().Be(password);
        command.FirstName.Should().Be(firstName);
        command.LastName.Should().Be(lastName);
        command.TelegramUserId.Should().Be(telegramUserId);
        command.TelegramUsername.Should().Be(telegramUsername);
        command.Localization.Should().Be(localization);
    }

    [Fact]
    public void RegisterUserCommand_WithoutTelegramData_ShouldAllowNullValues()
    {
        // Arrange & Act
        var command = new RegisterUserCommand
        {
            Email = "john.doe@example.com",
            Password = "SecurePassword123!",
            FirstName = "John",
            LastName = "Doe",
            TelegramUserId = null,
            TelegramUsername = null,
            Localization = "en-US"
        };

        // Assert
        command.TelegramUserId.Should().BeNull();
        command.TelegramUsername.Should().BeNull();
    }

    [Theory]
    [InlineData("john.doe@example.com")]
    [InlineData("jane.smith@company.org")]
    [InlineData("user@domain.co.uk")]
    public void RegisterUserCommand_Email_ShouldAcceptValidEmailFormats(string email)
    {
        // Arrange & Act
        var command = new RegisterUserCommand
        {
            Email = email
        };

        // Assert
        command.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("fr-FR")]
    [InlineData("de-DE")]
    [InlineData("es-ES")]
    public void RegisterUserCommand_Localization_ShouldAcceptValidLocalizationValues(string localization)
    {
        // Arrange & Act
        var command = new RegisterUserCommand
        {
            Localization = localization
        };

        // Assert
        command.Localization.Should().Be(localization);
    }

    [Fact]
    public void RegisterUserCommand_IsRecord_ShouldSupportRecordSemantics()
    {
        // Arrange
        var command1 = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password",
            FirstName = "John",
            LastName = "Doe",
            Localization = "en-US"
        };

        var command2 = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "password",
            FirstName = "John",
            LastName = "Doe",
            Localization = "en-US"
        };

        // Act & Assert
        command1.Should().Be(command2); // Records support value equality
        command1.GetHashCode().Should().Be(command2.GetHashCode());
    }

    [Fact]
    public void RegisterUserCommand_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var command1 = new RegisterUserCommand
        {
            Email = "test1@example.com",
            Password = "password1",
            FirstName = "John",
            LastName = "Doe",
            Localization = "en-US"
        };

        var command2 = new RegisterUserCommand
        {
            Email = "test2@example.com",
            Password = "password2",
            FirstName = "Jane",
            LastName = "Smith",
            Localization = "fr-FR"
        };

        // Act & Assert
        command1.Should().NotBe(command2);
    }

    [Fact]
    public void RegisterUserCommand_ToString_ShouldReturnMeaningfulRepresentation()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var stringRepresentation = command.ToString();

        // Assert
        stringRepresentation.Should().NotBeNullOrEmpty();
        stringRepresentation.Should().Contain("RegisterUserCommand");
    }
}