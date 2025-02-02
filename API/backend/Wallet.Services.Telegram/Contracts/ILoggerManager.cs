namespace Wallet.Services.Telegram.Contracts;

/// <summary>
/// Represents a logging interface with methods to log messages of various severity levels.
/// Provides an abstraction for logging information, warnings, debug messages, and errors
/// to facilitate application monitoring and troubleshooting.
/// </summary>
public interface ILoggerManager {
    void LogInfo(string message);
    void LogWarn(string message);
    void LogDebug(string message);
    void LogError(string message);
}