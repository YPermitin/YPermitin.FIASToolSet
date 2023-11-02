namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Параметры адресных объектов
/// </summary>
public class AddressObjectParameter
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
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;
    
    /// <summary>
    /// ID завершившей транзакции
    /// </summary>
    public readonly int ChangeIdEnd;
    
    /// <summary>
    /// Тип параметра
    /// </summary>
    public readonly int TypeId;

    /// <summary>
    /// Значение параметра
    /// </summary>
    public readonly string Value;
    
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
    
    public AddressObjectParameter(int id, int objectId, int changeId, int changeIdEnd, int typeId,
        string value, DateOnly updateDate, DateOnly startDate, DateOnly endDate)
    {
        Id = id;
        ObjectId = objectId;
        ChangeId = changeId;
        ChangeIdEnd = changeIdEnd;
        TypeId = typeId;
        Value = value;
        UpdateDate = updateDate;
        StartDate = startDate;
        EndDate = endDate;
    }
}