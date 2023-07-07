using Microsoft.Extensions.Configuration;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

Console.WriteLine("Тестовая загрузка полного дистрибутива ФИАС.");

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var options = new FIASDistributionBrowserOptions(
    workingDirectory: configuration.GetValue("FIASToolSet:WorkingDirectory", string.Empty));
IFIASDistributionBrowser loader = new FIASDistributionBrowser(options);
var lastInfo = await loader.GetLastDistributionInfo();

DateTime downloadStarted = DateTime.UtcNow;
DateTime lastProgressShow = DateTime.MinValue;
double lastProgressPercentage = 0;
if (lastInfo != null)
{
    await lastInfo.DownloadDistributionByFileTypeAsync(
        DistributionFileType.GARFIASXmlComplete,
        (args) =>
        {
            double progress = Math.Round(args.ProgressPercentage, 2);

            double progressChanged = lastProgressPercentage - progress;

            var showProgressTimeLeft = DateTime.UtcNow - lastProgressShow;
            if (args.State != DownloadDistributionFileProgressChangedEventType.Downloading
                || showProgressTimeLeft.TotalSeconds > 10
                || progressChanged >= 1)
            {
                lastProgressPercentage = progress;
                lastProgressShow = DateTime.UtcNow;

                var downloadDuration = DateTime.UtcNow - downloadStarted;
                Console.WriteLine(
                    $"[{DateTime.UtcNow}] {args.State}. Progress: {progress} %. Seconds left: {Math.Round(downloadDuration.TotalSeconds, 2)}");
                if (args.ErrorInfo != null)
                {
                    Console.WriteLine(args.ErrorInfo.GetBaseException().Message);
                }
            }
        });
    
    lastInfo.ExtractDistributionFile(
        DistributionFileType.GARFIASXmlComplete,
        true);
}

Console.WriteLine("Для выхода нажмите любую клавишу...");
Console.ReadKey();