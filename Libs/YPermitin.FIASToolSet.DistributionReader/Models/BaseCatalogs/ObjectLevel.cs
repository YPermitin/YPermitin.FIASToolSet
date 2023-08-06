namespace YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

/// <summary>
/// Уровень адресного объекта
/// </summary>
public class ObjectLevel
{
    public readonly int Level;
    public readonly string Name;
    public readonly DateOnly StartDate;
    public readonly DateOnly EndDate;
    public readonly DateOnly UpdateDate;
    public readonly bool IsActive;

    public ObjectLevel(int level, string name, DateOnly startDate, DateOnly endDate,
        DateOnly updateDate, bool isActive)
    {
        Level = level;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        UpdateDate = updateDate;
        IsActive = isActive;
    }
    
    public override string ToString()
    {
        return $"{Name} ({Level}, {StartDate}, {EndDate}, {UpdateDate}, {IsActive})";
    }
}