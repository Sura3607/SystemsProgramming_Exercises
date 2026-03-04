using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string _folderPath = @"C:\TradeFiles";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service started at: {time}", DateTime.Now);
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Checking folder at: {time}", DateTime.Now);

            if (Directory.Exists(_folderPath))
            {
                var files = Directory.GetFiles(_folderPath);
                _logger.LogInformation("Found {count} trade files", files.Length);
            }
            else
            {
                _logger.LogWarning("Folder not found: {path}", _folderPath);
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service stopping at: {time}", DateTime.Now);
        return base.StopAsync(cancellationToken);
    }
}