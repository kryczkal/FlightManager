using System.Net;
using Microsoft.VisualBasic;

namespace projob;

public enum LogLevel
{
    // Log levels in increasing order of severity
    Debug,
    Info,
    Error
}
public interface ILoggerHandler
{
    ILoggerHandler? SetNext(ILoggerHandler? handler);
    void Handle(string message, LogLevel logLevel);
}

public abstract class BaseHandler : ILoggerHandler
{
    private ILoggerHandler? _nextHandler;
    public ILoggerHandler? SetNext(ILoggerHandler? handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual void Handle(string message, LogLevel logLevel)
    {
        if (_nextHandler != null)
        {
            _nextHandler.Handle(message, logLevel);
        }
    }
}

public class ConsoleHandler : BaseHandler
{
    // ConsoleHandler doesn't require concurrency protection as it's thread safe
    private LogLevel _minLogLevel;

    public ConsoleHandler(LogLevel minLogLevel)
    {
        _minLogLevel = minLogLevel;
    }
    public override void Handle(string message, LogLevel logLevel)
    {
        if (logLevel >= _minLogLevel)
        {
            Console.WriteLine(Settings.LoggerSettings.LogFormat, DateTime.Now, logLevel.ToString(), message);
        }
        base.Handle(message, logLevel);
    }
}

public class FileHandler : BaseHandler
{
    private string _logFilePath;
    private LogLevel _minLogLevel;
    public FileHandler(LogLevel minLogLevel, string logFilePath)
    {
        _minLogLevel = minLogLevel;
        _logFilePath = logFilePath;
    }
    public override void Handle(string message, LogLevel logLevel)
    {
        // FileHandler requires concurrency protection
        if (logLevel >= _minLogLevel)
        {
            lock (_logFilePath)
            {
                File.AppendAllLines(_logFilePath, new[] { string.Format(Settings.LoggerSettings.LogFormat, DateTime.Now, logLevel.ToString(), message) });
            }
        }
        base.Handle(message, logLevel);
    }
}


public class Logger
{
    private readonly ILoggerHandler? _handler;
    public Logger(ILoggerHandler? handler)
    {
        _handler = handler;
    }

    public void Log(string message, LogLevel logLevel)
    {
        _handler?.Handle(message, logLevel);
    }
}

public static class GlobalLogger
{
    private static Logger _logger = new(Settings.LoggerSettings.LoggerChain);
    public static void Log(string message, LogLevel logLevel)
    {
        _logger.Log(message, logLevel);
    }
}