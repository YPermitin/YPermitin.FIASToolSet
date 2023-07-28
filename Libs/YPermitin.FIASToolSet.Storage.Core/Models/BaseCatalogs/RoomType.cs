namespace YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

/// <summary>
/// Тип помещения
/// </summary>
public class RoomType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsActive { get; set; }
}