using NLog;
using Wallet.Domain.Contracts;

namespace Wallet.Infrastructure.LoggerService;

public class LoggerManager : ILoggerManager {
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private static void LogToConsole(string level, string message) {
        Console.WriteLine($"{level}: {message}");
    }

    public void LogDebug(string message) {
        Logger.Debug(message);
        LogToConsole("DEBUG", message);
    }

    public void LogError(string message) {
        Logger.Error(message);
        LogToConsole("ERROR", message);
    }

    public void LogInfo(string message) {
        Logger.Info(message);
        LogToConsole("INFO", message);
    }

    public void LogWarn(string message) {
        Logger.Warn(message);
        LogToConsole("WARNING", message);
    }
}