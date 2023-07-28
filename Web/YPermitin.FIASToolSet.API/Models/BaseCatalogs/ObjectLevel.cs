namespace YPermitin.FIASToolSet.API.Models.BaseCatalogs;

/// <summary>
/// Уровень адресного объекта
/// </summary>
public class ObjectLevel
{
    public int Level { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsActive { get; set; }
}