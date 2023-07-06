// See https://aka.ms/new-console-template for more information

using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionBrowser.Enums;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

Console.WriteLine("Тестовая загрузка полного дистрибутива ФИАС.");

IFIASDistributionBrowser loader = new FIASDistributionBrowser();
var lastInfo = await loader.GetLastDistributionInfo();
var xmlFullUrl = lastInfo?.GetUrlByFileType(DistributionFileType.GARFIASXmlComplete);
            
var tempFileToDownload = Path.Combine(
    Path.GetTempPath(),
    "FIAS_FULL.zip");

DateTime downloadStarted = DateTime.UtcNow;
DateTime lastProgressShow = DateTime.MinValue;
double lastProgressPercentage = 0;
await lastInfo?.DownloadDistributionByFileTypeAsync(
    DistributionFileType.GARFIASXmlComplete,
    tempFileToDownload,
    (args) =>
    {
        double progress = Math.Round(args.ProgressPercentage, 2);

        double progressChanged = lastProgressPercentage - progress;
        
        var showProgressTimeLeft = DateTime.UtcNow - lastProgressShow;
        if (args.State != DownloadDistributionFileProgressChangedEventType.Downloading
            || showProgressTimeLeft.TotalSeconds > 10
            || progressChanged >= 1)
        {
            var downloadDuration = DateTime.UtcNow - downloadStarted;
            Console.WriteLine($"[{DateTime.UtcNow}] {args.State}. Progress: {progress} %. Seconds left: {downloadDuration.TotalSeconds}");
            if (args.ErrorInfo != null)
            {
                Console.WriteLine(args.ErrorInfo.GetBaseException().Message);
            }

            lastProgressPercentage = progress;
            lastProgressShow = DateTime.UtcNow;
        }
    });

Console.WriteLine("Для выхода нажмите любую клавишу...");
Console.ReadKey();