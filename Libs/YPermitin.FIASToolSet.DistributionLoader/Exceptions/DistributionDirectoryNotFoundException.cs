namespace YPermitin.FIASToolSet.DistributionLoader.Exceptions;

public class DistributionDirectoryNotFoundException : Exception
{
    public readonly string DirectoryPath;
    
    public DistributionDirectoryNotFoundException(string message, string directoryPath) : base(message)
    {
        DirectoryPath = directoryPath;
    }
}