namespace YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

/// <summary>
/// Тип нормативного документа
/// </summary>
public class NormativeDocType
{
    public readonly int Id;
    public readonly string Name;
    public readonly DateOnly StartDate;
    public readonly DateOnly EndDate;

    public NormativeDocType(int id, string name, DateOnly startDate, DateOnly endDate)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public override string ToString()
    {
        return $"{Name} ({Id}, {StartDate}, {EndDate})";
    }
}