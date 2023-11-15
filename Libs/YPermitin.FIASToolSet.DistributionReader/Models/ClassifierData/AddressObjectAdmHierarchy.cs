namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Иерархия административного деления адресных объектов
/// </summary>
public class AddressObjectAdmHierarchy
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
    /// Код региона
    /// </summary>
    public readonly int RegionCode;

    /// <summary>
    /// Код района
    /// </summary>
    public readonly int AreaCode;

    /// <summary>
    /// Код города
    /// </summary>
    public readonly int CityCode;

    /// <summary>
    /// Код населенного пункта
    /// </summary>
    public readonly int PlaceCode;

    /// <summary>
    /// Код ЭПС
    /// </summary>
    public readonly int PlanCode;

    /// <summary>
    /// Код улицы
    /// </summary>
    public readonly int StreetCode;
    
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
    
    public AddressObjectAdmHierarchy(int id, int objectId, int parentObjectId, int changeId,
        int regionCode, int areaCode, int cityCode, int placeCode, int planCode, int streetCode,
        int? previousAddressObjectId, int? nextAddressObjectId,
        DateOnly updateDate, DateOnly startDate, DateOnly endDate,
        bool isActive, string path)
    {
        Id = id;
        ObjectId = objectId;
        ParentObjectId = parentObjectId;
        ChangeId = changeId;
        RegionCode = regionCode;
        AreaCode = areaCode;
        CityCode = cityCode;
        PlaceCode = placeCode;
        PlanCode = planCode;
        StreetCode = streetCode;
        PreviousAddressObjectId = previousAddressObjectId;
        NextAddressObjectId = nextAddressObjectId;
        UpdateDate = updateDate;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = isActive;
        Path = path;
    }
}