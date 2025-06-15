using FluentAssertions;
using TestUtilities.Extensions;
using Xunit;

namespace TestUtilities.UnitTests.Extensions;

public class TestDataExtensionsTests
{
    [Fact]
    public void GenerateRandomString_ShouldReturnStringOfDefaultLength()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomString();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveLength(10);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(100)]
    public void GenerateRandomString_ShouldReturnStringOfSpecifiedLength(int length)
    {
        // Act
        var result = TestDataExtensions.GenerateRandomString(length);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveLength(length);
    }

    [Fact]
    public void GenerateRandomString_ShouldReturnDifferentStrings_OnMultipleCalls()
    {
        // Act
        var result1 = TestDataExtensions.GenerateRandomString();
        var result2 = TestDataExtensions.GenerateRandomString();

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void GenerateRandomEmail_ShouldReturnValidEmailFormat()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomEmail();

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain("@");
        result.Should().Contain(".");
        result.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    [Fact]
    public void GenerateRandomEmail_ShouldReturnDifferentEmails_OnMultipleCalls()
    {
        // Act
        var result1 = TestDataExtensions.GenerateRandomEmail();
        var result2 = TestDataExtensions.GenerateRandomEmail();

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void GenerateRandomName_ShouldReturnNonEmptyString()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomName();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Contain(" "); // Should contain space for full name
    }

    [Fact]
    public void GenerateRandomName_ShouldReturnDifferentNames_OnMultipleCalls()
    {
        // Act
        var result1 = TestDataExtensions.GenerateRandomName();
        var result2 = TestDataExtensions.GenerateRandomName();

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void GenerateRandomUsername_ShouldReturnNonEmptyString()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomUsername();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().NotContain(" "); // Username should not contain spaces
    }

    [Fact]
    public void GenerateRandomUsername_ShouldReturnDifferentUsernames_OnMultipleCalls()
    {
        // Act
        var result1 = TestDataExtensions.GenerateRandomUsername();
        var result2 = TestDataExtensions.GenerateRandomUsername();

        // Assert
        result1.Should().NotBe(result2);
    }

    [Fact]
    public void GenerateRandomDecimal_ShouldReturnValueWithinDefaultRange()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomDecimal();

        // Assert
        result.Should().BeGreaterOrEqualTo(0);
        result.Should().BeLessOrEqualTo(1000);
    }

    [Theory]
    [InlineData(10, 50)]
    [InlineData(100, 200)]
    [InlineData(0.01, 0.99)]
    public void GenerateRandomDecimal_ShouldReturnValueWithinSpecifiedRange(decimal min, decimal max)
    {
        // Act
        var result = TestDataExtensions.GenerateRandomDecimal(min, max);

        // Assert
        result.Should().BeGreaterOrEqualTo(min);
        result.Should().BeLessOrEqualTo(max);
    }

    [Fact]
    public void GenerateRandomDecimal_ShouldReturnDifferentValues_OnMultipleCalls()
    {
        // Act
        var results = new List<decimal>();
        for (int i = 0; i < 10; i++)
        {
            results.Add(TestDataExtensions.GenerateRandomDecimal());
        }

        // Assert
        results.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateRandomInt_ShouldReturnValueWithinDefaultRange()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomInt();

        // Assert
        result.Should().BeGreaterOrEqualTo(1);
        result.Should().BeLessOrEqualTo(1000000);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(100, 200)]
    [InlineData(1000, 2000)]
    public void GenerateRandomInt_ShouldReturnValueWithinSpecifiedRange(int min, int max)
    {
        // Act
        var result = TestDataExtensions.GenerateRandomInt(min, max);

        // Assert
        result.Should().BeGreaterOrEqualTo(min);
        result.Should().BeLessOrEqualTo(max);
    }

    [Fact]
    public void GenerateRandomInt_ShouldReturnDifferentValues_OnMultipleCalls()
    {
        // Act
        var results = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            results.Add(TestDataExtensions.GenerateRandomInt());
        }

        // Assert
        results.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateRandomLong_ShouldReturnValueWithinDefaultRange()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomLong();

        // Assert
        result.Should().BeGreaterOrEqualTo(1);
        result.Should().BeLessOrEqualTo(1000000000);
    }

    [Theory]
    [InlineData(1L, 100L)]
    [InlineData(1000L, 2000L)]
    [InlineData(1000000L, 2000000L)]
    public void GenerateRandomLong_ShouldReturnValueWithinSpecifiedRange(long min, long max)
    {
        // Act
        var result = TestDataExtensions.GenerateRandomLong(min, max);

        // Assert
        result.Should().BeGreaterOrEqualTo(min);
        result.Should().BeLessOrEqualTo(max);
    }

    [Fact]
    public void GenerateRandomLong_ShouldReturnDifferentValues_OnMultipleCalls()
    {
        // Act
        var results = new List<long>();
        for (int i = 0; i < 10; i++)
        {
            results.Add(TestDataExtensions.GenerateRandomLong());
        }

        // Assert
        results.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateRandomDateTime_ShouldReturnRecentDateTime()
    {
        // Arrange
        var now = DateTime.Now;

        // Act
        var result = TestDataExtensions.GenerateRandomDateTime();

        // Assert
        result.Should().BeBefore(now.AddMinutes(1)); // Allow small buffer for execution time
        result.Should().BeAfter(now.AddDays(-7)); // Bogus generates recent dates
    }

    [Fact]
    public void GenerateRandomDateTime_ShouldReturnDifferentValues_OnMultipleCalls()
    {
        // Act
        var results = new List<DateTime>();
        for (int i = 0; i < 10; i++)
        {
            results.Add(TestDataExtensions.GenerateRandomDateTime());
        }

        // Assert
        results.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void GenerateRandomFutureDateTime_ShouldReturnFutureDate()
    {
        // Arrange
        var now = DateTime.Now;

        // Act
        var result = TestDataExtensions.GenerateRandomFutureDateTime();

        // Assert
        result.Should().BeAfter(now);
    }

    [Fact]
    public void GenerateRandomPastDateTime_ShouldReturnPastDate()
    {
        // Arrange
        var now = DateTime.Now;

        // Act
        var result = TestDataExtensions.GenerateRandomPastDateTime();

        // Assert
        result.Should().BeBefore(now);
    }

    [Fact]
    public void GenerateRandomEnum_ShouldReturnValidEnumValue()
    {
        // Act
        var result = TestDataExtensions.GenerateRandomEnum<DayOfWeek>();

        // Assert
        result.Should().BeOneOf(Enum.GetValues<DayOfWeek>());
    }

    [Fact]
    public void GenerateRandomEnum_ShouldReturnDifferentValues_OnMultipleCalls()
    {
        // Act
        var results = new List<DayOfWeek>();
        for (int i = 0; i < 20; i++) // More iterations to ensure variety
        {
            results.Add(TestDataExtensions.GenerateRandomEnum<DayOfWeek>());
        }

        // Assert
        results.Should().ContainInOrder(results.Distinct()); // Should have some variety
        results.Count.Should().BeGreaterThan(results.Distinct().Count()); // Some duplicates expected
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GenerateRandomString_ShouldHandleZeroAndNegativeLength(int length)
    {
        // Act & Assert
        if (length <= 0)
        {
            var action = () => TestDataExtensions.GenerateRandomString(length);
            action.Should().NotThrow(); // Bogus handles this gracefully
        }
    }

    [Fact]
    public void GenerateRandomDecimal_WithEqualMinMax_ShouldReturnThatValue()
    {
        // Act
        const decimal value = 42.5m;
        var result = TestDataExtensions.GenerateRandomDecimal(value, value);

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void GenerateRandomInt_WithEqualMinMax_ShouldReturnThatValue()
    {
        // Arrange
        const int value = 42;

        // Act
        var result = TestDataExtensions.GenerateRandomInt(value, value);

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public void GenerateRandomLong_WithEqualMinMax_ShouldReturnThatValue()
    {
        // Arrange
        const long value = 42L;

        // Act
        var result = TestDataExtensions.GenerateRandomLong(value, value);

        // Assert
        result.Should().Be(value);
    }
} 