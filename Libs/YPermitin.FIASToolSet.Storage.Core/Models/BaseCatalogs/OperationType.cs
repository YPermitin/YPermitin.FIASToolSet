namespace YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

/// <summary>
/// Тип операции
/// </summary>
public class OperationType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsActive { get; set; }
}