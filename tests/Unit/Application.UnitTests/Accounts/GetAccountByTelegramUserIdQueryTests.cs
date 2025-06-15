using Application.Accounts.GetAccountByTelegramUserId;
using FluentAssertions;
using SharedKernel.DTO.Accounts;
using Xunit;

namespace Application.UnitTests.Accounts;

public class GetAccountByTelegramUserIdQueryTests
{
    [Fact]
    public void GetAccountByTelegramUserIdQuery_ShouldHaveCorrectProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramUserId = 123456789L;

        // Act
        var query = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Assert
        query.userid.Should().Be(userId);
        query.TelegramUserId.Should().Be(telegramUserId);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_ShouldBeAssignableToIQueryOfAccountResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramUserId = 123456789L;

        // Act
        var query = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Assert
        query.Should().BeAssignableTo<Application.Messaging.IQuery<AccountResponse>>();
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(999999999L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(0L)]
    public void GetAccountByTelegramUserIdQuery_ShouldAcceptVariousTelegramUserIds(long telegramUserId)
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Assert
        query.TelegramUserId.Should().Be(telegramUserId);
        query.userid.Should().Be(userId);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_ShouldBeRecord()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramUserId = 123456789L;
        var query1 = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);
        var query2 = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Assert - Records have value equality
        query1.Should().Be(query2);
        query1.Should().NotBeSameAs(query2);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_WithDifferentUserIds_ShouldNotBeEqual()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var telegramUserId = 123456789L;
        var query1 = new GetAccountByTelegramUserIdQuery(userId1, telegramUserId);
        var query2 = new GetAccountByTelegramUserIdQuery(userId2, telegramUserId);

        // Assert
        query1.Should().NotBe(query2);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_WithDifferentTelegramUserIds_ShouldNotBeEqual()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query1 = new GetAccountByTelegramUserIdQuery(userId, 123456789L);
        var query2 = new GetAccountByTelegramUserIdQuery(userId, 987654321L);

        // Assert
        query1.Should().NotBe(query2);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_ShouldGenerateCorrectHashCode()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramUserId = 123456789L;
        var query1 = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);
        var query2 = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Assert
        query1.GetHashCode().Should().Be(query2.GetHashCode());
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_ToString_ShouldContainBothIds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramUserId = 123456789L;
        var query = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Act
        var stringRepresentation = query.ToString();

        // Assert
        stringRepresentation.Should().Contain(userId.ToString());
        stringRepresentation.Should().Contain(telegramUserId.ToString());
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_WithEmptyGuid_ShouldAcceptValue()
    {
        // Arrange
        var telegramUserId = 123456789L;

        // Act
        var query = new GetAccountByTelegramUserIdQuery(Guid.Empty, telegramUserId);

        // Assert
        query.userid.Should().Be(Guid.Empty);
        query.TelegramUserId.Should().Be(telegramUserId);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_WithNegativeTelegramUserId_ShouldAcceptValue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var telegramUserId = -123456789L;

        // Act
        var query = new GetAccountByTelegramUserIdQuery(userId, telegramUserId);

        // Assert
        query.userid.Should().Be(userId);
        query.TelegramUserId.Should().Be(telegramUserId);
    }

    [Fact]
    public void GetAccountByTelegramUserIdQuery_WithZeroTelegramUserId_ShouldAcceptValue()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query = new GetAccountByTelegramUserIdQuery(userId, 0L);

        // Assert
        query.userid.Should().Be(userId);
        query.TelegramUserId.Should().Be(0L);
    }
}