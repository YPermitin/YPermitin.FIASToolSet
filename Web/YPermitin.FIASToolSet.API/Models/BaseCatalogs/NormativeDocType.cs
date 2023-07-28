namespace YPermitin.FIASToolSet.API.Models.BaseCatalogs;

/// <summary>
/// Тип нормативного документа
/// </summary>
public class NormativeDocType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}