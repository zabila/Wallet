﻿using NLog;
using Wallet.Domain.Contracts;

namespace Wallet.Infrastructure.LoggerService;

public class LoggerManager : ILoggerManager {
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private static void LogToConsole(string level, string message) {
        Console.WriteLine($"{level}: {message}");
    }

    public void LogDebug(string message) {
        _logger.Debug(message);
        LogToConsole("DEBUG", message);
    }

    public void LogError(string message) {
        _logger.Error(message);
        LogToConsole("ERROR", message);
    }

    public void LogInfo(string message) {
        _logger.Info(message);
        LogToConsole("INFO", message);
    }

    public void LogWarn(string message) {
        _logger.Warn(message);
        LogToConsole("WARNING", message);
    }
}