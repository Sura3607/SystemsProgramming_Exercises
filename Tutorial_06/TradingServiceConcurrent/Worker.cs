using System.Collections.Concurrent;
using System.Text;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private FileSystemWatcher _watcher;
    private readonly ConcurrentDictionary<string, bool> _processing = new();
    private readonly SemaphoreSlim _semaphore = new(4);
    private TradingServiceConfig _config;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _config = RegistryConfigReader.Load(_logger);
        EnsureDirectoriesExist();

        _watcher = new FileSystemWatcher(_config.InputFolder, "*.json");
        _watcher.Created += async (s, e) =>
        {
            await HandleFileAsync(e.FullPath, stoppingToken);
        };

        _watcher.EnableRaisingEvents = true;

        _logger.LogInformation("Watching folder: {folder}", _config.InputFolder);

        return Task.CompletedTask;
    }

    private async Task HandleFileAsync(string path, CancellationToken token)
    {
        if (!_processing.TryAdd(path, true))
            return;

        await _semaphore.WaitAsync(token);

        try
        {
            await WaitUntilReady(path, token);

            string content = await File.ReadAllTextAsync(path, Encoding.UTF8, token);

            _logger.LogInformation("Processing: {file}", path);

            await Task.Delay(2000, token); // simulate work

            string dest = Path.Combine(
                _config.ProcessedFolder,
                Path.GetFileName(path));

            File.Move(path, dest, true);

            _logger.LogInformation("Moved to processed: {file}", dest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file");
        }
        finally
        {
            _processing.TryRemove(path, out _);
            _semaphore.Release();
        }
    }

    private async Task WaitUntilReady(string path, CancellationToken token)
    {
        while (true)
        {
            try
            {
                using FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
                break;
            }
            catch
            {
                await Task.Delay(500, token);
            }
        }
    }

    private void EnsureDirectoriesExist()
    {
        try
        {
            if (!Directory.Exists(_config.InputFolder))
            {
                Directory.CreateDirectory(_config.InputFolder);
                _logger.LogInformation("Created Input folder: {folder}", _config.InputFolder);
            }

            if (!Directory.Exists(_config.ProcessedFolder))
            {
                Directory.CreateDirectory(_config.ProcessedFolder);
                _logger.LogInformation("Created Processed folder: {folder}", _config.ProcessedFolder);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Không th? t?o th? m?c c?u hình.");
            throw; // fail fast
        }
    }

    public override void Dispose()
    {
        _watcher?.Dispose();
        _semaphore?.Dispose();
        base.Dispose();
    }
}