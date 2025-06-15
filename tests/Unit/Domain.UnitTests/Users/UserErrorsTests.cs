using Domain.Users;
using FluentAssertions;
using SharedKernel;
using Xunit;

namespace Domain.UnitTests.Users;

public class UserErrorsTests
{
    [Fact]
    public void NotFoundByEmail_ShouldReturnNotFoundError()
    {
        // Act
        var error = UserErrors.NotFoundByEmail;

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Users.NotFoundByEmail");
        error.Description.Should().Be("The user with the specified email was not found");
    }

    [Fact]
    public void EmailNotUnique_ShouldReturnConflictError()
    {
        // Act
        var error = UserErrors.EmailNotUnique;

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.Conflict);
        error.Code.Should().Be("Users.EmailNotUnique");
        error.Description.Should().Be("The provided email is not unique");
    }

    [Fact]
    public void CannotUpdateUser_ShouldReturnProblemError()
    {
        // Act
        var error = UserErrors.CannotUpdateUser;

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.Problem);
        error.Code.Should().Be("Users.CannotUpdateUser");
        error.Description.Should().Be("The user cannot be updated");
    }

    [Fact]
    public void AnyTelegreamUserNotFound_ShouldReturnNotFoundError()
    {
        // Act
        var error = UserErrors.AnyTelegreamUserNotFound;

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("TelegreamUser.AnyNotFound");
        error.Description.Should().Be("The user was not found");
    }

    [Fact]
    public void NotLinkedWithUser_ShouldReturnNotFoundError()
    {
        // Act
        var error = UserErrors.NotLinkedWithUser;

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("TelegreamUser.NotLinkedWithUser");
        error.Description.Should().Be("The user is not linked with any user");
    }

    [Fact]
    public void NotFound_WithUserId_ShouldReturnNotFoundErrorWithSpecificMessage()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var error = UserErrors.NotFound(userId);

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Users.NotFound");
        error.Description.Should().Be($"The user with the Id = '{userId}' was not found");
    }

    [Fact]
    public void TelegreamUserNotFound_WithUserId_ShouldReturnNotFoundErrorWithSpecificMessage()
    {
        // Arrange
        var userId = 123456789L;

        // Act
        var error = UserErrors.TelegreamUserNotFound(userId);

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("TelegreamUser.NotFound");
        error.Description.Should().Be($"The user with the Id = '{userId}' was not found");
    }

    [Fact]
    public void Unauthorized_ShouldReturnFailureError()
    {
        // Act
        var error = UserErrors.Unauthorized();

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.Failure);
        error.Code.Should().Be("Users.Unauthorized");
        error.Description.Should().Be("You are not authorized to perform this action.");
    }

    [Theory]
    [InlineData("User already exists")]
    [InlineData("Password too weak")]
    [InlineData("Email validation failed")]
    public void OnReginsterUser_WithReason_ShouldReturnProblemErrorWithSpecificMessage(string reason)
    {
        // Act
        var error = UserErrors.OnReginsterUser(reason);

        // Assert
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.Problem);
        error.Code.Should().Be("Users.RegisterUser");
        error.Description.Should().Be($"The user cannot be added, Reason: {reason}");
    }

    [Fact]
    public void TelegreamUserNotFound_WithDifferentUserIds_ShouldReturnUniqueMessages()
    {
        // Arrange
        var userId1 = 111111111L;
        var userId2 = 222222222L;

        // Act
        var error1 = UserErrors.TelegreamUserNotFound(userId1);
        var error2 = UserErrors.TelegreamUserNotFound(userId2);

        // Assert
        error1.Description.Should().NotBe(error2.Description);
        error1.Description.Should().Contain(userId1.ToString());
        error2.Description.Should().Contain(userId2.ToString());
    }

    [Fact]
    public void NotFound_WithDifferentUserIds_ShouldReturnUniqueMessages()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        // Act
        var error1 = UserErrors.NotFound(userId1);
        var error2 = UserErrors.NotFound(userId2);

        // Assert
        error1.Description.Should().NotBe(error2.Description);
        error1.Description.Should().Contain(userId1.ToString());
        error2.Description.Should().Contain(userId2.ToString());
    }
}