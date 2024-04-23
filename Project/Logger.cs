namespace projob;

public enum LogLevel
{
    Info,
    Debug,
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
    public override void Handle(string message, LogLevel logLevel)
    {
        if (logLevel == LogLevel.Info)
        {
            Console.WriteLine($"Info: {message}");
        }
        else if (logLevel == LogLevel.Debug)
        {
            Console.WriteLine($"Debug: {message}");
        }
        else if (logLevel == LogLevel.Error)
        {
            Console.WriteLine($"Error: {message}");
        }
        base.Handle(message, logLevel);
    }
}

public class FileHandler : BaseHandler
{
    private string _logFilePath;
    public FileHandler(string logFilePath)
    {
        _logFilePath = logFilePath;
    }
    public override void Handle(string message, LogLevel logLevel)
    {
        if (logLevel == LogLevel.Info)
        {
            File.WriteAllText(_logFilePath, $"Info: {message}");
        }
        else if (logLevel == LogLevel.Debug)
        {
            File.WriteAllText(_logFilePath, $"Debug: {message}");
        }
        else if (logLevel == LogLevel.Error)
        {
            File.WriteAllText(_logFilePath, $"Error: {message}");
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