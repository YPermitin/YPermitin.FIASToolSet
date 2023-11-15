namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Иерархия административного деления адресных объектов
/// </summary>
public class AddressObjectAdmHierarchy
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Глобальный уникальный идентификатор адресного объекта
    /// </summary>
    public int ObjectId { get; set; }
    
    /// <summary>
    /// Идентификатор родительского объекта 
    /// </summary>
    public int ParentObjectId { get; set; }
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public int ChangeId { get; set; }

    /// <summary>
    /// Код региона
    /// </summary>
    public int? RegionCode { get; set; }

    /// <summary>
    /// Код района
    /// </summary>
    public int? AreaCode { get; set; }

    /// <summary>
    /// Код города
    /// </summary>
    public int? CityCode { get; set; }

    /// <summary>
    /// Код населенного пункта
    /// </summary>
    public int? PlaceCode { get; set; }

    /// <summary>
    /// Код ЭПС
    /// </summary>
    public int? PlanCode { get; set; }

    /// <summary>
    /// Код улицы
    /// </summary>
    public int? StreetCode { get; set; }
    
    /// <summary>
    /// Идентификатор записи связывания с предыдущей исторической записью
    /// </summary>        
    public int? PreviousAddressObjectId { get; set; }
    
    /// <summary>
    /// Идентификатор записи связывания с последующей исторической записью
    /// </summary>        
    public int? NextAddressObjectId { get; set; }
    
    /// <summary>
    /// Дата внесения (обновления) записи
    /// </summary>
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// Начало действия записи
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Окончание действия записи
    /// </summary>
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// Признак действующего адресного объекта
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Материализованный путь к объекту (полная иерархия)
    /// </summary>
    public string Path { get; set; }
}