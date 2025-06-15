using Domain.Users;
using FluentAssertions;
using TestUtilities.Builders;
using Xunit;

namespace TestUtilities.UnitTests.Builders;

public class UserBuilderTests
{
    [Fact]
    public void Create_ShouldReturnNewUserBuilderInstance()
    {
        // Act
        var builder = UserBuilder.Create();

        // Assert
        builder.Should().NotBeNull();
        builder.Should().BeOfType<UserBuilder>();
    }

    [Fact]
    public void Build_ShouldCreateUserWithDefaultValues()
    {
        // Arrange
        var builder = UserBuilder.Create();

        // Act
        var user = builder.Build();

        // Assert
        user.Should().NotBeNull();
        user.Should().BeOfType<User>();
        user.Email.Should().Be("test@example.com");
        user.FirstName.Should().Be("Test");
        user.LastName.Should().Be("User");
        user.Localization.Should().Be("en-US");
    }

    [Fact]
    public void WithTelegramUserId_ShouldSetTelegramUserId()
    {
        // Arrange
        const long telegramUserId = 999999;
        var builder = UserBuilder.Create();

        // Act
        var user = builder.WithTelegramUserId(telegramUserId).Build();

        // Assert
        user.TelegramUser?.TelegramUserId.Should().Be(telegramUserId);
    }

    [Fact]
    public void WithFirstName_ShouldSetFirstName()
    {
        // Arrange
        const string firstName = "Custom";
        var builder = UserBuilder.Create();

        // Act
        var user = builder.WithFirstName(firstName).Build();

        // Assert
        user.FirstName.Should().Be(firstName);
    }

    [Fact]
    public void WithLastName_ShouldSetLastName()
    {
        // Arrange
        const string lastName = "Name";
        var builder = UserBuilder.Create();

        // Act
        var user = builder.WithLastName(lastName).Build();

        // Assert
        user.LastName.Should().Be(lastName);
    }

    [Fact]
    public void WithEmail_ShouldSetEmail()
    {
        // Arrange
        const string email = "custom@example.com";
        var builder = UserBuilder.Create();

        // Act
        var user = builder.WithEmail(email).Build();

        // Assert
        user.Email.Should().Be(email);
        user.UserName.Should().Be(email); // UserName should match Email
    }

    [Fact]
    public void WithId_ShouldReturnBuilderInstance_ForFluentInterface()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var builder = UserBuilder.Create();

        // Act
        var result = builder.WithId(userId);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void WithCreatedAt_ShouldReturnBuilderInstance_ForFluentInterface()
    {
        // Arrange
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var builder = UserBuilder.Create();

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
        var builder = UserBuilder.Create();

        // Act
        var result = builder.WithUpdatedAt(updatedAt);

        // Assert
        result.Should().BeSameAs(builder);
    }

    [Fact]
    public void FluentInterface_ShouldAllowChaining()
    {
        // Arrange
        const long telegramUserId = 123456;
        const string firstName = "Fluent";
        const string lastName = "User";
        const string email = "fluent@example.com";
        var createdAt = DateTime.UtcNow.AddHours(-1);

        // Act
        var user = UserBuilder.Create()
            .WithTelegramUserId(telegramUserId)
            .WithFirstName(firstName)
            .WithLastName(lastName)
            .WithEmail(email)
            .WithCreatedAt(createdAt)
            .Build();

        // Assert
        user.TelegramUser?.TelegramUserId.Should().Be(telegramUserId);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void Build_ShouldCreateDifferentUserInstances_WhenCalledMultipleTimes()
    {
        // Arrange
        var builder = UserBuilder.Create();

        // Act
        var user1 = builder.Build();
        var user2 = builder.Build();

        // Assert
        user1.Should().NotBeSameAs(user2);
        user1.Id.Should().NotBe(user2.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void WithFirstName_ShouldHandleEmptyOrNullValues(string firstName)
    {
        // Arrange
        var builder = UserBuilder.Create();

        // Act & Assert
        var action = () => builder.WithFirstName(firstName).Build();
        
        // The behavior depends on domain validation - this test documents expected behavior
        action.Should().NotThrow(); // The User entity doesn't enforce validation here
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void WithEmail_ShouldHandleEmptyOrNullValues(string email)
    {
        // Arrange
        var builder = UserBuilder.Create();

        // Act & Assert
        var action = () => builder.WithEmail(email).Build();
        
        // The behavior depends on domain validation - this test documents expected behavior
        action.Should().NotThrow(); // The User entity doesn't enforce validation here
    }

    [Fact]
    public void Build_ShouldCreateValidUser_WithMinimalRequiredProperties()
    {
        // Arrange
        const string email = "minimal@example.com";

        // Act
        var user = UserBuilder.Create()
            .WithEmail(email)
            .WithFirstName("U")
            .WithLastName("ser")
            .Build();

        // Assert
        user.Should().NotBeNull();
        user.Email.Should().Be(email);
        user.FirstName.Should().Be("U");
        user.LastName.Should().Be("ser");
    }

    [Fact]
    public void Build_ShouldCreateUserWithTelegramUser_WhenTelegramDataProvided()
    {
        // Arrange
        const long telegramUserId = 123456789;
        const string telegramUsername = "testuser";

        // Act
        var user = UserBuilder.Create()
            .WithTelegramUserId(telegramUserId)
            .WithTelegramUsername(telegramUsername)
            .Build();

        // Assert
        user.TelegramUser.Should().NotBeNull();
        user.TelegramUser!.TelegramUserId.Should().Be(telegramUserId);
        user.TelegramUser.TelegramUsername.Should().Be(telegramUsername);
        user.TelegramId.Should().Be(user.TelegramUser.Id);
    }

    [Fact]
    public void Build_ShouldNotCreateTelegramUser_WhenTelegramDataMissing()
    {
        // Act
        var user = UserBuilder.Create()
            .WithTelegramUserId(null)
            .Build();

        // Assert
        user.TelegramUser.Should().BeNull();
        user.TelegramId.Should().BeNull();
    }
} 