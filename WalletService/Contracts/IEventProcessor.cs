namespace Contracts;

public interface IEventProcessor
{
    Task ProcessEvent(string message);
}