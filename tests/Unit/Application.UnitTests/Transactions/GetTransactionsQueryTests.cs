using Application.Transactions.GetTransactions;
using FluentAssertions;
using SharedKernel.DTO.Transactions;
using Xunit;

namespace Application.UnitTests.Transactions;

public class GetTransactionsQueryTests
{
    [Fact]
    public void GetTransactionsQuery_ShouldHaveCorrectUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query = new GetTransactionsQuery(userId);

        // Assert
        query.UserId.Should().Be(userId);
    }

    [Fact]
    public void GetTransactionsQuery_ShouldBeAssignableToIQueryOfListTransactionsResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query = new GetTransactionsQuery(userId);

        // Assert
        query.Should().BeAssignableTo<Application.Messaging.IQuery<List<TransactionsResponse>>>();
    }

    [Fact]
    public void GetTransactionsQuery_ShouldBeRecord()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query1 = new GetTransactionsQuery(userId);
        var query2 = new GetTransactionsQuery(userId);

        // Assert - Records have value equality
        query1.Should().Be(query2);
        query1.Should().NotBeSameAs(query2);
    }

    [Fact]
    public void GetTransactionsQuery_WithDifferentUserIds_ShouldNotBeEqual()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var query1 = new GetTransactionsQuery(userId1);
        var query2 = new GetTransactionsQuery(userId2);

        // Assert
        query1.Should().NotBe(query2);
    }

    [Fact]
    public void GetTransactionsQuery_ShouldGenerateCorrectHashCode()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query1 = new GetTransactionsQuery(userId);
        var query2 = new GetTransactionsQuery(userId);

        // Assert
        query1.GetHashCode().Should().Be(query2.GetHashCode());
    }

    [Fact]
    public void GetTransactionsQuery_ToString_ShouldContainUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetTransactionsQuery(userId);

        // Act
        var stringRepresentation = query.ToString();

        // Assert
        stringRepresentation.Should().Contain(userId.ToString());
    }

    [Fact]
    public void GetTransactionsQuery_WithEmptyGuid_ShouldAcceptValue()
    {
        // Act
        var query = new GetTransactionsQuery(Guid.Empty);

        // Assert
        query.UserId.Should().Be(Guid.Empty);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("123e4567-e89b-12d3-a456-426614174000")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void GetTransactionsQuery_ShouldAcceptVariousGuidFormats(string guidString)
    {
        // Arrange
        var userId = Guid.Parse(guidString);

        // Act
        var query = new GetTransactionsQuery(userId);

        // Assert
        query.UserId.Should().Be(userId);
    }

    [Fact]
    public void GetTransactionsQuery_WithNewGuid_ShouldCreateCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query = new GetTransactionsQuery(userId);

        // Assert
        query.UserId.Should().Be(userId);
        query.UserId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void GetTransactionsQuery_MultipleInstances_WithSameUserId_ShouldBeEqual()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var query1 = new GetTransactionsQuery(userId);
        var query2 = new GetTransactionsQuery(userId);
        var query3 = new GetTransactionsQuery(userId);

        // Assert
        query1.Should().Be(query2);
        query2.Should().Be(query3);
        query1.Should().Be(query3);
    }

    [Fact]
    public void GetTransactionsQuery_MultipleInstances_WithDifferentUserIds_ShouldNotBeEqual()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var userId3 = Guid.NewGuid();

        // Act
        var query1 = new GetTransactionsQuery(userId1);
        var query2 = new GetTransactionsQuery(userId2);
        var query3 = new GetTransactionsQuery(userId3);

        // Assert
        query1.Should().NotBe(query2);
        query2.Should().NotBe(query3);
        query1.Should().NotBe(query3);
    }
}