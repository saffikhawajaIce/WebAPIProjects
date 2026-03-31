using System;

namespace HostManagementAPI;

//this is the logger service which is used to log the messages in the application,
//and it uses the built-in ILogger<T> interface to log the messages to the console, 
//and it also allows for logging of exceptions with a message.

public interface ILoggerService
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message, Exception exception = null);
    void LogDebug(string message);
}

public class LoggerService : ILoggerService
{
    private readonly ILogger<LoggerService> _logger;

    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    public void LogError(string message, Exception exception = null)
    {
        if (exception != null)
        {
            _logger.LogError(exception, message);
        }
        else
        {
            _logger.LogError(message);
        }
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(message);
    }
}
