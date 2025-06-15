using Application.Users.GetUserByTelegramId;
using FluentAssertions;
using SharedKernel.DTO.Users;
using Xunit;

namespace Application.UnitTests.Users;

public class GetUserByTelegramIdQueryTests
{
    [Fact]
    public void GetUserByTelegramIdQuery_ShouldHaveCorrectTelegramId()
    {
        // Arrange
        var telegramId = 123456789L;

        // Act
        var query = new GetUserByTelegramIdQuery(telegramId);

        // Assert
        query.TelegramId.Should().Be(telegramId);
    }

    [Fact]
    public void GetUserByTelegramIdQuery_ShouldBeAssignableToIQueryOfUserResponse()
    {
        // Arrange
        var telegramId = 123456789L;

        // Act
        var query = new GetUserByTelegramIdQuery(telegramId);

        // Assert
        query.Should().BeAssignableTo<Application.Messaging.IQuery<UserResponse>>();
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(999999999L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public void GetUserByTelegramIdQuery_ShouldAcceptVariousTelegramIds(long telegramId)
    {
        // Act
        var query = new GetUserByTelegramIdQuery(telegramId);

        // Assert
        query.TelegramId.Should().Be(telegramId);
    }

    [Fact]
    public void GetUserByTelegramIdQuery_ShouldBeRecord()
    {
        // Arrange
        var telegramId = 123456789L;
        var query1 = new GetUserByTelegramIdQuery(telegramId);
        var query2 = new GetUserByTelegramIdQuery(telegramId);

        // Assert - Records have value equality
        query1.Should().Be(query2);
        query1.Should().NotBeSameAs(query2);
    }

    [Fact]
    public void GetUserByTelegramIdQuery_WithDifferentTelegramIds_ShouldNotBeEqual()
    {
        // Arrange
        var query1 = new GetUserByTelegramIdQuery(123456789L);
        var query2 = new GetUserByTelegramIdQuery(987654321L);

        // Assert
        query1.Should().NotBe(query2);
    }

    [Fact]
    public void GetUserByTelegramIdQuery_ShouldGenerateCorrectHashCode()
    {
        // Arrange
        var telegramId = 123456789L;
        var query1 = new GetUserByTelegramIdQuery(telegramId);
        var query2 = new GetUserByTelegramIdQuery(telegramId);

        // Assert
        query1.GetHashCode().Should().Be(query2.GetHashCode());
    }

    [Fact]
    public void GetUserByTelegramIdQuery_ToString_ShouldContainTelegramId()
    {
        // Arrange
        var telegramId = 123456789L;
        var query = new GetUserByTelegramIdQuery(telegramId);

        // Act
        var stringRepresentation = query.ToString();

        // Assert
        stringRepresentation.Should().Contain(telegramId.ToString());
    }
}