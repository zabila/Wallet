using AutoFixture;
using AutoFixture.Xunit2;

namespace TestUtilities.Fixtures;

public class TestFixture
{
    protected readonly IFixture Fixture;

    public TestFixture()
    {
        Fixture = new Fixture();
        ConfigureFixtureInternal();
    }

    private void ConfigureFixtureInternal()
    {
        // Configure AutoFixture for common scenarios
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        // Call virtual method after base configuration
        ConfigureFixture();
    }

    protected virtual void ConfigureFixture()
    {
        // Override in derived classes for custom configuration
    }

    protected T Create<T>() => Fixture.Create<T>();

    protected T[] CreateMany<T>(int count = 3) => Fixture.CreateMany<T>(count).ToArray();

    protected T Build<T>(Action<T> configure) where T : class
    {
        var instance = Create<T>();
        configure(instance);
        return instance;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class CustomAutoDataAttribute : AutoDataAttribute
{
    public CustomAutoDataAttribute() : base(() => new Fixture())
    {
    }
} 