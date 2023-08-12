namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Переподчинение адресных объектов
/// </summary>
public class AddressObjectDivision
{
    /// <summary>
    /// Уникальный идентификатор записи.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Глобальный уникальный идентификатор родительского адресного объекта 
    /// </summary>
    public int ParentId { get; set; }
    
    /// <summary>
    /// Глобальный уникальный идентификатор дочернего адресного объекта
    /// </summary>
    public int ChildId { get; set; }
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public int ChangeId { get; set; }
}