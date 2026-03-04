public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TradingServiceConfig config;
        
        try
        {
            config = RegistryConfigReader.Load(_logger);
            _logger.LogInformation("Đọc cấu hình thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Service không thể khởi động.");
            throw;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processing folder: {Folder}", config.InputFolder);

            await Task.Delay(
                TimeSpan.FromSeconds(config.IntervalSeconds),
                stoppingToken);
        }
    }
}