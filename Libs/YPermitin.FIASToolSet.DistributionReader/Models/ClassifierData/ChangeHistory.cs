using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// История изменения адресных объектов
/// </summary>
public class ChangeHistory
{
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public readonly int ChangeId;
    
    /// <summary>
    /// Глобальный уникальный идентификатор объекта
    /// </summary>
    public readonly int ObjectId;
    
    /// <summary>
    /// Глобальный уникальный идентификатор ID изменившей транзакции
    /// Соответствует полю AOID выгрузки в формате ФИАС
    /// </summary>
    public readonly Guid AddressObjectGuid;
    
    /// <summary>
    /// ID статуса действия
    /// </summary>
    public readonly int OperationTypeId;
    
    /// <summary>
    /// Идентификатор нормативного документа
    /// </summary>
    public readonly int NormativeDocId;

    /// <summary>
    /// Дата изменения
    /// </summary>
    public readonly DateOnly ChangeDate;
    
    public ChangeHistory(int objectId, Guid addressObjectGuid, int changeId, int operationTypeId, 
        int normativeDocId, DateOnly changeDate)
    {
        ObjectId = objectId;
        AddressObjectGuid = addressObjectGuid;
        ChangeId = changeId;
        OperationTypeId = operationTypeId;
        NormativeDocId = normativeDocId;
        ChangeDate = changeDate;
    }
}