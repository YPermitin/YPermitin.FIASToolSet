namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Строение
/// </summary>
public class House
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
    /// Глобальный уникальный идентификатор адресного объекта
    /// типа UUID
    /// </summary>
    public readonly Guid ObjectGuid;
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;

    /// <summary>
    /// Основной номер строения 
    /// </summary>
    public readonly string HouseNumber;

    /// <summary>
    /// Дополнительный номер дома 1
    /// </summary>
    public readonly string AddedHouseNumber1;

    /// <summary>
    /// Дополнительный номер дома 2
    /// </summary>
    public readonly string AddedHouseNumber2;

    /// <summary>
    /// Основной тип дома
    /// </summary>
    public readonly int HouseTypeId;
    
    /// <summary>
    /// Дополнительный тип дома 1
    /// </summary>
    public readonly int AddedHouseTypeId1;

    /// <summary>
    /// Дополнительный тип дома 2
    /// </summary>
    public readonly int AddedHouseTypeId2;

    /// <summary>
    /// Статус действия над записью – причина появления записи
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
    /// </summary>
    public readonly bool IsActual;
    
    /// <summary>
    /// Признак действующего адресного объекта
    /// </summary>
    public readonly bool IsActive;
    
    public House(int id, int objectId, Guid objectGuid, int changeId,
        string houseNumber, string addedHouseNumber1, string addedHouseNumber2, 
        int houseTypeId, int addedHouseTypeId1, int addedHouseTypeId2,
        int operationTypeId, int? previousAddressObjectId, int? nextAddressObjectId,
        DateOnly updateDate, DateOnly startDate, DateOnly endDate,
        bool isActive, bool isActual)
    {
        Id = id;
        ObjectId = objectId;
        ObjectGuid = objectGuid;
        ChangeId = changeId;
        HouseNumber = houseNumber;
        AddedHouseNumber1 = addedHouseNumber1;
        AddedHouseNumber2 = addedHouseNumber2;
        HouseTypeId = houseTypeId;
        AddedHouseTypeId1 = addedHouseTypeId1;
        AddedHouseTypeId2 = addedHouseTypeId2;
        OperationTypeId = operationTypeId;
        PreviousAddressObjectId = previousAddressObjectId;
        NextAddressObjectId = nextAddressObjectId;
        UpdateDate = updateDate;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = isActive;
        IsActual = isActual;
    }
}