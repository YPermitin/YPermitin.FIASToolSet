namespace YPermitin.FIASToolSet.DistributionReader.Exceptions;

public class FIASDataNotFoundException : Exception
{
    private readonly string FilePath;

    public FIASDataNotFoundException(string message, string filePath) : base(message)
    {
        FilePath = filePath;
    }
}