using FluentAssertions;
using SharedKernel;
using SharedKernel.Abstractions;
using Xunit;

namespace Domain.UnitTests.SharedKernel;

// Test implementation of IDomainEvent for testing purposes
public record TestDomainEvent(string Message) : IDomainEvent;

// Test implementation of Entity for testing purposes
public class TestEntity : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class EntityTests
{
    [Fact]
    public void Entity_Constructor_ShouldInitializeEmptyDomainEvents()
    {
        // Act
        var entity = new TestEntity();

        // Assert
        entity.DomainEvents.Should().NotBeNull();
        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Raise_WithDomainEvent_ShouldAddEventToCollection()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent("Test message");

        // Act
        entity.Raise(domainEvent);

        // Assert
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.Should().Contain(domainEvent);
    }

    [Fact]
    public void Raise_WithMultipleDomainEvents_ShouldAddAllEventsToCollection()
    {
        // Arrange
        var entity = new TestEntity();
        var event1 = new TestDomainEvent("First message");
        var event2 = new TestDomainEvent("Second message");
        var event3 = new TestDomainEvent("Third message");

        // Act
        entity.Raise(event1);
        entity.Raise(event2);
        entity.Raise(event3);

        // Assert
        entity.DomainEvents.Should().HaveCount(3);
        entity.DomainEvents.Should().Contain(event1);
        entity.DomainEvents.Should().Contain(event2);
        entity.DomainEvents.Should().Contain(event3);
    }

    [Fact]
    public void ClearDomainEvents_WithEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var entity = new TestEntity();
        var event1 = new TestDomainEvent("First message");
        var event2 = new TestDomainEvent("Second message");
        entity.Raise(event1);
        entity.Raise(event2);

        // Act
        entity.ClearDomainEvents();

        // Assert
        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void ClearDomainEvents_WithEmptyEvents_ShouldRemainEmpty()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.ClearDomainEvents();

        // Assert
        entity.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void DomainEvents_ShouldReturnImmutableCopy()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent("Test message");
        entity.Raise(domainEvent);

        // Act
        var events = entity.DomainEvents;

        // Attempting to modify the returned collection should not affect the original
        var originalCount = events.Count;

        // Assert
        events.Should().HaveCount(1);
        events.Should().Contain(domainEvent);

        // The returned collection should be a copy
        events.Count.Should().Be(originalCount);
    }

    [Fact]
    public void Raise_WithSameDomainEventMultipleTimes_ShouldAddMultipleInstances()
    {
        // Arrange
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent("Same message");

        // Act
        entity.Raise(domainEvent);
        entity.Raise(domainEvent);
        entity.Raise(domainEvent);

        // Assert
        entity.DomainEvents.Should().HaveCount(3);
        entity.DomainEvents.Should().AllBeEquivalentTo(domainEvent);
    }

    [Fact]
    public void Raise_WithNullDomainEvent_ShouldThrowArgumentNullException()
    {
        // Arrange
        var entity = new TestEntity();

        // Act & Assert
        var action = () => entity.Raise(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Entity_RaiseAndClear_Workflow_ShouldWorkCorrectly()
    {
        // Arrange
        var entity = new TestEntity();
        var event1 = new TestDomainEvent("Event 1");
        var event2 = new TestDomainEvent("Event 2");

        // Act & Assert - Add events
        entity.Raise(event1);
        entity.Raise(event2);
        entity.DomainEvents.Should().HaveCount(2);

        // Act & Assert - Clear events
        entity.ClearDomainEvents();
        entity.DomainEvents.Should().BeEmpty();

        // Act & Assert - Add new events after clearing
        var event3 = new TestDomainEvent("Event 3");
        entity.Raise(event3);
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents.Should().Contain(event3);
    }
}