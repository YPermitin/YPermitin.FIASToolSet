namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Иерархия в муниципальном делении адресных объектов
/// </summary>
public class MunHierarchy
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public readonly int Id;
    
    /// <summary>
    /// Глобальный уникальный идентификатор адресного объекта
    /// </summary>
    public readonly int ObjectId;
    
    /// <summary>
    /// Идентификатор родительского объекта 
    /// </summary>
    public readonly int ParentObjectId;
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;

    /// <summary>
    /// Код ОКТМО
    /// </summary>
    public readonly string OKTMO;
    
    /// <summary>
    /// Идентификатор записи связывания с предыдущей исторической записью
    /// </summary>        
    public readonly int? PreviousAddressObjectId;
    
    /// <summary>
    /// Идентификатор записи связывания с последующей исторической записью
    /// </summary>        
    public readonly int? NextAddressObjectId;
    
    /// <summary>
    /// Дата внесения (обновления) записи
    /// </summary>
    public readonly DateOnly UpdateDate;

    /// <summary>
    /// Начало действия записи
    /// </summary>
    public readonly DateOnly StartDate;

    /// <summary>
    /// Окончание действия записи
    /// </summary>
    public readonly DateOnly EndDate;
    
    /// <summary>
    /// Признак действующего адресного объекта
    /// </summary>
    public readonly bool IsActive;

    /// <summary>
    /// Материализованный путь к объекту (полная иерархия)
    /// </summary>
    public readonly string Path;
    
    public MunHierarchy(int id, int objectId, int parentObjectId, int changeId,
        string oktmo, int? previousAddressObjectId, int? nextAddressObjectId,
        DateOnly updateDate, DateOnly startDate, DateOnly endDate,
        bool isActive, string path)
    {
        Id = id;
        ObjectId = objectId;
        ParentObjectId = parentObjectId;
        ChangeId = changeId;
        OKTMO = oktmo;
        PreviousAddressObjectId = previousAddressObjectId;
        NextAddressObjectId = nextAddressObjectId;
        UpdateDate = updateDate;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = isActive;
        Path = path;
    }
}