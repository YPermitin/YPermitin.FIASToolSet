namespace YPermitin.FIASToolSet.API.Models.BaseCatalogs;

/// <summary>
/// Тип адресного объекта
/// </summary>
public class AddressObjectType
{
    public int Id { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime UpdateDate { get; set; }
}