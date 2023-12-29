using YPermitin.FIASToolSet.DistributionReader.Helpers;

namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Реестр адресных элементов
/// </summary>
public class ObjectRegistry
{
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
    /// Признак действующего объекта
    /// </summary>
    public readonly bool IsActive;

    /// <summary>
    /// Уровень объекта
    /// </summary>
    public readonly int LevelId;
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public readonly DateOnly CreateDate;
    
    /// <summary>
    /// Дата изменения
    /// </summary>
    public readonly DateOnly UpdateDate;

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
                        .Add(ObjectId)
                        .Add(ObjectGuid)
                        .Add(ChangeId)
                        .Add(IsActive)
                        .Add(LevelId)
                        .Add(CreateDate.ToDateTime(TimeOnly.MinValue))
                        .Add(UpdateDate.ToDateTime(TimeOnly.MinValue))
                        .Build();
                }
            }
            
            return _hashMD5;
        }
    }
    
    public ObjectRegistry(int objectId, Guid objectGuid, int changeId, bool isActive,
        int levelId, DateOnly createDate, DateOnly updateDate)
    {
        ObjectId = objectId;
        ObjectGuid = objectGuid;
        ChangeId = changeId;
        IsActive = isActive;
        LevelId = levelId;
        CreateDate = createDate;
        UpdateDate = updateDate;
    }
}