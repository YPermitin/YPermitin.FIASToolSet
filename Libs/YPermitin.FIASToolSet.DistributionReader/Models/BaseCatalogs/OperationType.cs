namespace YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

/// <summary>
/// Тип операции
/// </summary>
public class OperationType
{
    public readonly int Id;
    public readonly string Name;
    public readonly DateOnly StartDate;
    public readonly DateOnly EndDate;
    public readonly DateOnly UpdateDate;
    public readonly bool IsActive;

    public OperationType(int id, string name,
        DateOnly startDate, DateOnly endDate, DateOnly updateDate, bool isActive)
    {
        Id = id;
        Name = name;
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