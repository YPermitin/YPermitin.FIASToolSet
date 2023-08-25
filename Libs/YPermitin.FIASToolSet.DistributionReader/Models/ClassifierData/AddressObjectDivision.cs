namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Переподчинение адресных объектов
/// </summary>
public class AddressObjectDivision
{
    /// <summary>
    /// Уникальный идентификатор записи.
    /// </summary>
    public readonly int Id;

    /// <summary>
    /// Глобальный уникальный идентификатор родительского адресного объекта 
    /// </summary>
    public readonly int ParentId;

    /// <summary>
    /// Глобальный уникальный идентификатор дочернего адресного объекта
    /// </summary>
    public readonly int ChildId;

    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;

    public AddressObjectDivision(int id, int parentId, int childId, int changeId)
    {
        Id = id;
        ParentId = parentId;
        ChildId = childId;
        ChangeId = changeId;
    }
}