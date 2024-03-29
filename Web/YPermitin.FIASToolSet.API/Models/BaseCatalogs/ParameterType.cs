namespace YPermitin.FIASToolSet.API.Models.BaseCatalogs;

/// <summary>
/// Тип параметра
/// </summary>
public class ParameterType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsActive { get; set; }
}