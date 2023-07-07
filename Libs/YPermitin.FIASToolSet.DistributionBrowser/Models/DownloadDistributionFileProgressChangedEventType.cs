namespace YPermitin.FIASToolSet.DistributionBrowser.Models;

public enum DownloadDistributionFileProgressChangedEventType
{
    Started,
    Downloading,
    Compleated,
    Failure,
    Canceled,
    AlreadyExists
}