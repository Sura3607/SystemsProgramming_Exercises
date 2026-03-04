using Microsoft.Win32;
using Microsoft.Extensions.Logging;

public static class RegistryConfigReader
{
    private const string PathKey = @"SOFTWARE\TradingService";

    public static TradingServiceConfig Load(ILogger logger)
    {
        using var key = Registry.LocalMachine.OpenSubKey(PathKey);

        if (key == null)
            throw new Exception("Registry key không tồn tại");

        var input = key.GetValue("InputFolder") as string;
        var processed = key.GetValue("ProcessedFolder") as string;
        var intervalObj = key.GetValue("IntervalSeconds");

        if (string.IsNullOrWhiteSpace(input) ||
            string.IsNullOrWhiteSpace(processed) ||
            intervalObj == null)
            throw new Exception("Cấu hình thiếu giá trị");

        if (!int.TryParse(intervalObj.ToString(), out int interval) || interval <= 0)
            throw new Exception("Interval không hợp lệ");

        return new TradingServiceConfig
        {
            InputFolder = input,
            ProcessedFolder = processed,
            IntervalSeconds = interval
        };
    }
}