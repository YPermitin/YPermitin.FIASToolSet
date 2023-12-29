using YPermitin.FIASToolSet.DistributionReader.Helpers;
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

    public byte[] _hashMD5;
    /// <summary>
    /// MD5-хэш элемента
    /// </summary>
    public byte[] HashMD5
    {
        get
        {
            if (_hashMD5 == null)
            {
                using (var md5Builder = new MD5Builder())
                {
                    _hashMD5 = md5Builder
                        .Add(ChangeId)
                        .Add(ObjectId)
                        .Add(AddressObjectGuid)
                        .Add(ChangeDate.ToDateTime(TimeOnly.MinValue))
                        .Add(OperationTypeId)
                        .Add(NormativeDocId)
                        .Build();
                }
            }
            
            return _hashMD5;
        }
    }
    
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