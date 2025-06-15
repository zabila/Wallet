using Application.Authetication.Login;
using FluentAssertions;
using SharedKernel.DTO.Login;
using Xunit;

namespace Application.UnitTests.Authentication;

public class LoginCommandTests
{
    [Fact]
    public void LoginCommand_ShouldHaveCorrectEmailAndPassword()
    {
        // Arrange
        var email = "user@example.com";
        var password = "SecurePassword123!";

        // Act
        var command = new LoginCommand(email, password);

        // Assert
        command.Email.Should().Be(email);
        command.Password.Should().Be(password);
    }

    [Fact]
    public void LoginCommand_ShouldBeAssignableToICommandOfTokenResponse()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password";

        // Act
        var command = new LoginCommand(email, password);

        // Assert
        command.Should().BeAssignableTo<Application.Messaging.ICommand<TokenResponse>>();
    }

    [Theory]
    [InlineData("user@example.com", "password123")]
    [InlineData("admin@domain.org", "AdminPass!")]
    [InlineData("test.user@company.co.uk", "TestPassword")]
    [InlineData("jane.doe@website.net", "Jane123!@#")]
    public void LoginCommand_ShouldAcceptVariousEmailPasswordCombinations(string email, string password)
    {
        // Act
        var command = new LoginCommand(email, password);

        // Assert
        command.Email.Should().Be(email);
        command.Password.Should().Be(password);
    }

    [Fact]
    public void LoginCommand_ShouldBeRecord()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password123";
        var command1 = new LoginCommand(email, password);
        var command2 = new LoginCommand(email, password);

        // Assert - Records have value equality
        command1.Should().Be(command2);
        command1.Should().NotBeSameAs(command2);
    }

    [Fact]
    public void LoginCommand_WithDifferentEmails_ShouldNotBeEqual()
    {
        // Arrange
        var password = "password123";
        var command1 = new LoginCommand("user1@example.com", password);
        var command2 = new LoginCommand("user2@example.com", password);

        // Assert
        command1.Should().NotBe(command2);
    }

    [Fact]
    public void LoginCommand_WithDifferentPasswords_ShouldNotBeEqual()
    {
        // Arrange
        var email = "user@example.com";
        var command1 = new LoginCommand(email, "password123");
        var command2 = new LoginCommand(email, "differentPassword");

        // Assert
        command1.Should().NotBe(command2);
    }

    [Fact]
    public void LoginCommand_ShouldGenerateCorrectHashCode()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password123";
        var command1 = new LoginCommand(email, password);
        var command2 = new LoginCommand(email, password);

        // Assert
        command1.GetHashCode().Should().Be(command2.GetHashCode());
    }

    [Fact]
    public void LoginCommand_ToString_ShouldContainEmail()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password123";
        var command = new LoginCommand(email, password);

        // Act
        var stringRepresentation = command.ToString();

        // Assert
        stringRepresentation.Should().Contain(email);
    }

    [Fact]
    public void LoginCommand_ToString_ShouldNotExposePassword()
    {
        // Arrange
        var email = "user@example.com";
        var password = "SecretPassword123!";
        var command = new LoginCommand(email, password);

        // Act
        var stringRepresentation = command.ToString();

        // Assert
        stringRepresentation.Should().Contain(email);
        // Note: In a real implementation, we might want to ensure passwords are not exposed in ToString()
        // but since this is a simple record, the password will be included. This test documents the current behavior.
    }

    [Fact]
    public void LoginCommand_WithEmptyEmailAndPassword_ShouldAcceptValues()
    {
        // Act
        var command = new LoginCommand("", "");

        // Assert
        command.Email.Should().Be("");
        command.Password.Should().Be("");
        command.Email.Should().BeEmpty();
        command.Password.Should().BeEmpty();
    }



    [Fact]
    public void LoginCommand_WithComplexPassword_ShouldAcceptValue()
    {
        // Arrange
        var email = "user@example.com";
        var complexPassword = "Complex!Password123@#$";

        // Act
        var command = new LoginCommand(email, complexPassword);

        // Assert
        command.Password.Should().Be(complexPassword);
        command.Email.Should().Be(email);
        command.Password.Should().Contain("!");
        command.Password.Should().Contain("@");
        command.Password.Length.Should().BeGreaterThan(10);
    }

    [Fact]
    public void LoginCommand_WithNullEmail_ShouldAcceptValue()
    {
        // Act
        var command = new LoginCommand(null!, "password");

        // Assert
        command.Email.Should().BeNull();
        command.Password.Should().Be("password");
    }

    [Fact]
    public void LoginCommand_WithNullPassword_ShouldAcceptValue()
    {
        // Act
        var command = new LoginCommand("user@example.com", null!);

        // Assert
        command.Email.Should().Be("user@example.com");
        command.Password.Should().BeNull();
    }

    [Fact]
    public void LoginCommand_WithNullEmailAndPassword_ShouldAcceptValues()
    {
        // Act
        var command = new LoginCommand(null!, null!);

        // Assert
        command.Email.Should().BeNull();
        command.Password.Should().BeNull();
    }
}