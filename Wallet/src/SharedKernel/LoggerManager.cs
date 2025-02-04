using NLog;
using SharedKernel.Abstractions;

namespace SharedKernel;

public class LoggerManager : ILoggerManager
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private static void LogToConsole(string level, string message)
    {
        Console.WriteLine($"{level}: {message}");
    }

    public void LogDebug(string message)
    {
        logger.Debug(message);
        LogToConsole("DEBUG", message);
    }

    public void LogError(string message)
    {
        logger.Error(message);
        LogToConsole("ERROR", message);
    }

    public void LogInfo(string message)
    {
        logger.Info(message);
        LogToConsole("INFO", message);
    }

    public void LogWarn(string message)
    {
        logger.Warn(message);
        LogToConsole("WARNING", message);
    }
}
