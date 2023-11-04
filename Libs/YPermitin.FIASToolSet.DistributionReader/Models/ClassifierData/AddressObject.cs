namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Адресный объект
/// </summary>
public class AddressObject
{
    /// <summary>
    /// Уникальный идентификатор записи.
    /// </summary>
    public readonly int Id;

    /// <summary>
    /// Глобальный уникальный идентификатор адресного объекта
    /// </summary>
    public readonly int ObjectId;

    /// <summary>
    /// Глобальный уникальный идентификатор адресного объекта
    /// типа UUID
    /// </summary>
    public readonly Guid ObjectGuid;

    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;

    /// <summary>
    /// Наименование
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Краткое наименование типа объекта
    /// </summary>
    public readonly string TypeName;

    /// <summary>
    /// Идентификатор уровня адресного объекта 
    /// </summary>        
    public readonly int LevelId;

    /// <summary>
    /// Идентификатор статуса действия над записью – причины появления записи
    /// </summary>        
    public readonly int OperationTypeId;

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
    /// Статус актуальности адресного объекта ФИАС
    ///
    /// У последней записи адресного объекта элемент всегда принимает значение True, у предыдущих False
    /// </summary>
    public readonly bool IsActual;

    /// <summary>
    /// Признак действующего адресного объекта
    /// </summary>
    public readonly bool IsActive;

    public AddressObject(int id, int objectId, Guid objectGuid, int changeId, 
        string name, string typeName, int levelId, int operationTypeId, 
        int? previousAddressObjectId, 
        int? nextAddressObjectId, 
        DateOnly updateDate, DateOnly startDate, DateOnly 
            endDate, bool isActual, bool isActive)
    {
        Id = id;
        ObjectId = objectId;
        ObjectGuid = objectGuid;
        ChangeId = changeId;
        Name = name;
        TypeName = typeName;
        LevelId = levelId;
        OperationTypeId = operationTypeId;
        PreviousAddressObjectId = previousAddressObjectId;
        NextAddressObjectId = nextAddressObjectId;
        UpdateDate = updateDate;
        StartDate = startDate;
        EndDate = endDate;
        IsActual = isActual;
        IsActive = isActive;
    }
}