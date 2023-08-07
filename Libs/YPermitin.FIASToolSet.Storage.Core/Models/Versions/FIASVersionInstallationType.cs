namespace YPermitin.FIASToolSet.Storage.Core.Models.Versions;

/// <summary>
/// Тип установки версии ФИАС
/// </summary>
public class FIASVersionInstallationType
{
    public static Guid Full = new Guid("e4c31e19-cb2d-47cd-b96e-08a0876ac4f6");
    public static Guid Update = new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d");

    /// <summary>
    /// Идентификатор статуса
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Наименование статуса
    /// </summary>
    public string Name { get; set; }
}