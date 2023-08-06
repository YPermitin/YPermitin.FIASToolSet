namespace YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

/// <summary>
/// Тип адресного объекта
/// </summary>
public class AddressObjectType
{
    public readonly int Id;
    public readonly int Level;
    public readonly string Name;
    public readonly string ShortName;
    public readonly string Description;
    public readonly DateOnly StartDate;
    public readonly DateOnly EndDate;
    public readonly DateOnly UpdateDate;

    public AddressObjectType(int id, int level, 
        string name, string shortName, string description, 
        DateOnly startDate, DateOnly endDate, DateOnly updateDate)
    {
        Id = id;
        Name = name;
        Level = level;
        ShortName = shortName;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        UpdateDate = updateDate;
    }
    
    public override string ToString()
    {
        return $"{Name} ({Id}, {Level}, {StartDate}, {EndDate}, {UpdateDate})";
    }
}