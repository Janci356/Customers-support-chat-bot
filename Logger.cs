using Serilog;
using Serilog.Formatting.Json;

namespace Customers_support_chat_bot;

public class Logger
{
    private static Logger? _instance;
    private readonly ILogger _logger;

    private Logger()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(new JsonFormatter(),
                "../../../logs/log.txt", 
                rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static Logger GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Logger();
        }

        return _instance;
    }

    public void LogInformation(string? message)
    {
        Task.Run(() => _logger.Information(message));
    }

    public void LogError(string? message)
    {
        Task.Run(() => _logger.Error(message));
    }
}
