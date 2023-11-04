namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Машино-место
/// </summary>
public class CarPlace
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public readonly int Id;
    
    /// <summary>
    /// Глобальный уникальный идентификатор объекта
    /// </summary>
    public readonly int ObjectId;
    
    /// <summary>
    /// Глобальный уникальный идентификатор объекта
    /// типа UUID
    /// </summary>
    public readonly Guid ObjectGuid;
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;
    
    /// <summary>
    /// Номер машино-места
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Статус действия над записью – причина появления записи
    /// </summary>
    public int OperationTypeId { get; set; }
    
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
    /// Статус актуальности объекта ФИАС
    /// </summary>
    public readonly bool IsActual;
    
    /// <summary>
    /// Признак действующего объекта
    /// </summary>
    public readonly bool IsActive;
    
    public CarPlace(int id, int objectId, Guid objectGuid, int changeId,
        string number, int operationTypeId,
        int? previousAddressObjectId, int? nextAddressObjectId,
        DateOnly updateDate, DateOnly startDate, DateOnly endDate,
        bool isActive, bool isActual)
    {
        Id = id;
        ObjectId = objectId;
        ObjectGuid = objectGuid;
        ChangeId = changeId;
        Number = number;
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