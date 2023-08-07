namespace YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

/// <summary>
/// Вид нормативного документа
/// </summary>
public class NormativeDocKind
{
    public readonly int Id;
    public readonly string Name;

    public NormativeDocKind(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }
}