using Domain.Users;
using FluentAssertions;
using SharedKernel;
using Xunit;

namespace Domain.UnitTests.Users;

public class TelegramUserTests
{
    [Fact]
    public void TelegramUser_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var telegramUserId = 123456789L;
        var telegramUsername = "johndoe";
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddMinutes(5);

        // Act
        var telegramUser = new TelegramUser
        {
            Id = id,
            TelegramUserId = telegramUserId,
            TelegramUsername = telegramUsername,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            UserId = userId
        };

        // Assert
        telegramUser.Id.Should().Be(id);
        telegramUser.TelegramUserId.Should().Be(telegramUserId);
        telegramUser.TelegramUsername.Should().Be(telegramUsername);
        telegramUser.CreatedAt.Should().Be(createdAt);
        telegramUser.UpdatedAt.Should().Be(updatedAt);
        telegramUser.UserId.Should().Be(userId);
    }

    [Fact]
    public void TelegramUser_ShouldInheritFromEntity()
    {
        // Arrange & Act
        var telegramUser = new TelegramUser();

        // Assert
        telegramUser.Should().BeAssignableTo<Entity>();
    }

    [Fact]
    public void TelegramUser_WithUser_ShouldSetRelationshipCorrectly()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var telegramUser = new TelegramUser
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 123456789L,
            TelegramUsername = "johndoe",
            UserId = user.Id,
            User = user
        };

        // Assert
        telegramUser.User.Should().Be(user);
        telegramUser.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void TelegramUser_ShouldAllowNullUser()
    {
        // Arrange & Act
        var telegramUser = new TelegramUser();

        // Assert
        telegramUser.User.Should().BeNull();
    }

    [Theory]
    [InlineData(123456789L)]
    [InlineData(987654321L)]
    [InlineData(1L)]
    public void TelegramUser_TelegramUserId_ShouldAcceptValidLongValues(long telegramUserId)
    {
        // Arrange & Act
        var telegramUser = new TelegramUser
        {
            TelegramUserId = telegramUserId
        };

        // Assert
        telegramUser.TelegramUserId.Should().Be(telegramUserId);
    }

    [Theory]
    [InlineData("john_doe")]
    [InlineData("user123")]
    [InlineData("")]
    public void TelegramUser_TelegramUsername_ShouldAcceptValidStringValues(string username)
    {
        // Arrange & Act
        var telegramUser = new TelegramUser
        {
            TelegramUsername = username
        };

        // Assert
        telegramUser.TelegramUsername.Should().Be(username);
    }
}