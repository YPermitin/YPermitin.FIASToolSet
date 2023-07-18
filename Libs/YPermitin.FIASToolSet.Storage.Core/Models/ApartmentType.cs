namespace YPermitin.FIASToolSet.Storage.Core.Models;

/// <summary>
/// Тип квартиры
/// </summary>
public class ApartmentType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsActive { get; set; }
}