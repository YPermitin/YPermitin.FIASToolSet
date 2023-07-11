namespace YPermitin.FIASToolSet.DistributionReader.Models;

/// <summary>
/// Тип строения
/// </summary>
public class HouseType
{
    public readonly int Id;
    public readonly string Name;
    public readonly string ShortName;
    public readonly string Description;
    public readonly DateOnly StartDate;
    public readonly DateOnly EndDate;
    public readonly DateOnly UpdateDate;
    public readonly bool IsActive;

    public HouseType(int id, string name, string shortName, string description, 
        DateOnly startDate, DateOnly endDate, DateOnly updateDate, bool isActive)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        UpdateDate = updateDate;
        IsActive = isActive;
    }

    public override string ToString()
    {
        return $"{Name} ({Id}, {StartDate}, {EndDate}, {UpdateDate}, {IsActive})";
    }
}