using Microsoft.Win32;
using Microsoft.Extensions.Logging;

public static class RegistryConfigReader
{
    private const string RegistryPath = @"SOFTWARE\TradingService";

    public static TradingServiceConfig Load(ILogger logger)
    {
        using var key = Registry.LocalMachine.OpenSubKey(RegistryPath);

        if (key == null)
        {
            logger.LogError("Registry key không tồn tại.");
            throw new Exception("Missing registry configuration.");
        }

        var inputFolder = key.GetValue("InputFolder") as string;
        var processedFolder = key.GetValue("ProcessedFolder") as string;
        var intervalObj = key.GetValue("IntervalSeconds");

        if (string.IsNullOrWhiteSpace(inputFolder) ||
            string.IsNullOrWhiteSpace(processedFolder) ||
            intervalObj == null)
        {
            logger.LogError("Thiếu giá trị cấu hình.");
            throw new Exception("Invalid configuration.");
        }

        if (!int.TryParse(intervalObj.ToString(), out int intervalSeconds) || intervalSeconds <= 0)
        {
            logger.LogError("IntervalSeconds không hợp lệ.");
            throw new Exception("Invalid interval value.");
        }

        return new TradingServiceConfig
        {
            InputFolder = inputFolder,
            ProcessedFolder = processedFolder,
            IntervalSeconds = intervalSeconds
        };
    }
}