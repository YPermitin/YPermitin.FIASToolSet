namespace YPermitin.FIASToolSet.DistributionReader.Models;

/// <summary>
/// Тип параметра
/// </summary>
public class ParameterType
{
    public readonly int Id;
    public readonly string Name;
    public readonly string Description;
    public readonly string Code;
    public readonly DateOnly StartDate;
    public readonly DateOnly EndDate;
    public readonly DateOnly UpdateDate;
    public readonly bool IsActive;

    public ParameterType(int id, string name, string description, string code, 
        DateOnly startDate, DateOnly endDate, DateOnly updateDate, bool isActive)
    {
        Id = id;
        Name = name;
        Description = description;
        Code = code;
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