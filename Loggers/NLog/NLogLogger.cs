using NLog;

namespace Loggers.NLog;

public class NLogLogger : ILogger
{
    private readonly Logger logger;

    public NLogLogger(string name)
    {
         logger = LogManager.GetLogger(name);
    }
    
    public void Info(string message, params object[]? args)
    {
        logger.Info(message, args);
    }

    public void Error(string message, params object[]? args)
    {
        logger.Error(message, args);
    }

    public void Error(Exception exception, string message, params object[]? args)
    {
        logger.Error(exception, message, args);
    }
}