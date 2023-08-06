namespace YPermitin.FIASToolSet.DistributionLoader.Exceptions;

public class RegionNotFoundException : Exception
{
    public readonly string RegionCode;
    
    public RegionNotFoundException(string message, string regionCode) : base(message)
    {
        RegionCode = regionCode;
    }
}