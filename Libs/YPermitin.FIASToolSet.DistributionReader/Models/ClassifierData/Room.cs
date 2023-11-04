namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Комната
/// </summary>
public class Room
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
    /// Номер комнаты или офиса
    /// </summary>
    public readonly string RoomNumber;
    
    /// <summary>
    /// Тип комнаты или офиса
    /// </summary>
    public readonly int RoomTypeId;
    
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
    
    public Room(int id, int objectId, Guid objectGuid, int changeId,
        string roomNumber, int roomTypeId, int operationTypeId, 
        int? previousAddressObjectId, int? nextAddressObjectId,
        DateOnly updateDate, DateOnly startDate, DateOnly endDate,
        bool isActive, bool isActual)
    {
        Id = id;
        ObjectId = objectId;
        ObjectGuid = objectGuid;
        ChangeId = changeId;
        RoomNumber = roomNumber;
        RoomTypeId = roomTypeId;
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