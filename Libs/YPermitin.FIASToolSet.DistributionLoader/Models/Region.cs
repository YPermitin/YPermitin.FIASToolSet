namespace YPermitin.FIASToolSet.DistributionLoader.Models;

/// <summary>
/// Информация о регионе
/// </summary>
public class Region
{
    /// <summary>
    /// Код региона
    /// </summary>
    public readonly int Code;

    public Region(int code)
    {
        Code = code;
    }
}