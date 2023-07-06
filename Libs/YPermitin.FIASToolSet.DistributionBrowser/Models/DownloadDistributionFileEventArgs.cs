using System;

namespace YPermitin.FIASToolSet.DistributionBrowser.Models;

public class DownloadDistributionFileProgressChangedEventArgs
{
    public DownloadDistributionFileProgressChangedEventType State { get; }
    public double ProgressPercentage { get; }
    public Exception ErrorInfo { get; }

    public DownloadDistributionFileProgressChangedEventArgs(
        DownloadDistributionFileProgressChangedEventType state, 
        double progressPercentage,
        Exception errorInfo = null)
    {
        ProgressPercentage = progressPercentage;
        State = state;
        ErrorInfo = errorInfo;
    }
}