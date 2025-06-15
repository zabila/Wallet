using FluentAssertions;
using SharedKernel;
using Xunit;

namespace Domain.UnitTests.SharedKernel;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Success_WithValue_ShouldCreateSuccessfulResultWithValue()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error message");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Failure_WithValue_ShouldCreateFailedResultWithDefaultValue()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error message");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Value_OnSuccessfulResult_ShouldReturnValue()
    {
        // Arrange
        var expectedValue = 42;
        var result = Result.Success(expectedValue);

        // Act
        var actualValue = result.Value;

        // Assert
        actualValue.Should().Be(expectedValue);
    }

    [Fact]
    public void Value_OnFailedResult_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error message");
        var result = Result.Failure<int>(error);

        // Act & Assert
        var action = () => result.Value;
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("The value of a failure result can't be accessed.");
    }

    [Fact]
    public void Constructor_WithSuccessAndError_ShouldThrowArgumentException()
    {
        // Arrange
        var error = Error.Failure("Test.Error", "Test error message");

        // Act & Assert
        var action = () => new Result(true, error);
        action.Should().Throw<ArgumentException>()
            .WithParameterName("error");
    }

    [Fact]
    public void Constructor_WithFailureAndNoError_ShouldThrowArgumentException()
    {
        // Act & Assert
        var action = () => new Result(false, Error.None);
        action.Should().Throw<ArgumentException>()
            .WithParameterName("error");
    }

    [Fact]
    public void ImplicitOperator_WithNonNullValue_ShouldCreateSuccessfulResult()
    {
        // Arrange
        var value = "test";

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void ImplicitOperator_WithNullValue_ShouldCreateFailedResult()
    {
        // Arrange
        string? value = null;

        // Act
        Result<string> result = value;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.NullValue);
    }

    [Fact]
    public void ValidationFailure_ShouldCreateFailedResultWithError()
    {
        // Arrange
        var error = new Error("Test.Validation", "Validation failed", ErrorType.Validation);

        // Act
        var result = Result<string>.ValidationFailure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsFailure_ShouldBeOppositeOfIsSuccess(bool isSuccess)
    {
        // Arrange
        var error = isSuccess ? Error.None : Error.Failure("Test.Error", "Test error");
        var result = new Result(isSuccess, error);

        // Act & Assert
        result.IsFailure.Should().Be(!isSuccess);
        result.IsSuccess.Should().Be(isSuccess);
    }

    [Fact]
    public void Result_WithComplexObject_ShouldHandleValueCorrectly()
    {
        // Arrange
        var complexObject = new { Name = "Test", Id = 123 };

        // Act
        var result = Result.Success(complexObject);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(complexObject);
        result.Value.Name.Should().Be("Test");
        result.Value.Id.Should().Be(123);
    }
}