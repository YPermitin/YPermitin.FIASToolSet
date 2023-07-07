using System.IO;

namespace YPermitin.FIASToolSet.DistributionBrowser.Models;

/// <summary>
/// Параметры обозревателя дистрибутивов ФИАС
/// </summary>
public class FIASDistributionBrowserOptions
{
    public static readonly FIASDistributionBrowserOptions Default;

    static FIASDistributionBrowserOptions()
    {
        string workingDirectory = Path.Combine(
            Path.GetTempPath(),
            "FIASToolSet");
        if (!Directory.Exists(workingDirectory))
        {
            Directory.CreateDirectory(workingDirectory);
        }

        Default = new FIASDistributionBrowserOptions(
            workingDirectory: workingDirectory);
    }
    
    /// <summary>
    /// Рабочий каталог
    ///
    /// Используется для хранения служебных и временных файлов.
    /// </summary>
    public readonly string WorkingDirectory;

    /// <summary>
    /// Максимальная скороть загрузки дистрибутивов ФИАС (байт в секунду).
    ///
    /// Значение 0 означает отсутствие ограничений. Используется по умолчанию.
    /// </summary>
    public readonly long MaximumDownloadSpeedBytesPerSecond;

    public FIASDistributionBrowserOptions(
        string workingDirectory = null, 
        long maximumDownloadSpeedBytesPerSecond = long.MinValue)
    {
        workingDirectory ??= Default.WorkingDirectory;
        
        WorkingDirectory = workingDirectory;
        MaximumDownloadSpeedBytesPerSecond = maximumDownloadSpeedBytesPerSecond;
    }
}