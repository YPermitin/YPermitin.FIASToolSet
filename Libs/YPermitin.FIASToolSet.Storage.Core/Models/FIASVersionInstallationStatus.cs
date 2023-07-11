namespace YPermitin.FIASToolSet.Storage.Core.Models;

/// <summary>
/// Статус установки версии ФИАС
/// </summary>
public class FIASVersionInstallationStatus
{
    public static Guid New = new Guid("090cc6b8-a5c3-451c-b8fd-e5522ba9ce6a");
    public static Guid Installing = new Guid("4dba445f-ff47-4071-b9ae-6d3c56d6fe7d");
    public static Guid Installed = new Guid("b0473a78-2743-4f64-b2ea-683b97cc55c5");
    
    /// <summary>
    /// Идентификатор статуса
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Наименование статуса
    /// </summary>
    public string Name { get; set; }
}