namespace YPermitin.FIASToolSet.DistributionReader.Models;

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