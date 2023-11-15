using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Строение
/// </summary>
public class House
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
    /// Глобальный уникальный идентификатор адресного объекта
    /// типа UUID
    /// </summary>
    public Guid ObjectGuid { get; set; }
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public int ChangeId { get; set; }

    /// <summary>
    /// Основной номер строения 
    /// </summary>
    public string HouseNumber { get; set; }

    /// <summary>
    /// Дополнительный номер дома 1
    /// </summary>
    public string AddedHouseNumber1 { get; set; }

    /// <summary>
    /// Дополнительный номер дома 2
    /// </summary>
    public string AddedHouseNumber2 { get; set; }

    /// <summary>
    /// Основной тип дома
    /// </summary>
    [ForeignKey("HouseTypeId")]
    public HouseType HouseType { get; set; }
    /// <summary>
    /// Идентификатор основного тип дома
    /// </summary>
    public int? HouseTypeId { get; set; }
    
    /// <summary>
    /// Дополнительный тип дома 1
    /// </summary>
    [ForeignKey("AddedHouseTypeId1")]
    public HouseType AddedHouseType1 { get; set; }
    /// <summary>
    /// Идентификатор дополнительного типа дома 1
    /// </summary>
    public int? AddedHouseTypeId1 { get; set; }

    /// <summary>
    /// Дополнительный тип дома 2
    /// </summary>
    [ForeignKey("AddedHouseTypeId2")]
    public HouseType AddedHouseType2 { get; set; }
    /// <summary>
    /// Идентификатор дополнительного типа дома 2
    /// </summary>
    public int? AddedHouseTypeId2 { get; set; }

    /// <summary>
    /// Статус действия над записью – причина появления записи
    /// </summary>
    [ForeignKey("OperationTypeId")]
    public OperationType OperationType { get; set; }
    /// <summary>
    /// Идентификатор статуса действия над записью – причина появления записи
    /// </summary>
    public int OperationTypeId { get; set; }
    
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
    /// Статус актуальности адресного объекта ФИАС
    /// </summary>
    public bool IsActual { get; set; }
    
    /// <summary>
    /// Признак действующего адресного объекта
    /// </summary>
    public bool IsActive { get; set; }
}