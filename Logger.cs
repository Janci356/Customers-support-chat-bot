using Serilog;

namespace Customers_support_chat_bot;

public class Logger
{
    private readonly ILogger _logger;

    public Logger()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("../../../logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public void LogInformation(string message)
    {
        Task.Run(() => _logger.Information(message)); // Log in the background
    }

    public void LogError(string message)
    {
        Task.Run(() => _logger.Error(message)); // Log in the background
    }
}
