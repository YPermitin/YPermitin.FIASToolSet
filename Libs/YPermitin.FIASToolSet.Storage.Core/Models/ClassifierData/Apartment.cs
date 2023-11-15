using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Помещение
/// </summary>
public class Apartment
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
    /// Номер комнаты
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Тип комнаты
    /// </summary>
    [ForeignKey("ApartmentTypeId")]
    public ApartmentType ApartmentType { get; set; }
    /// <summary>
    /// Идентификатор типа комнаты
    /// </summary>
    public int ApartmentTypeId { get; set; }

    /// <summary>
    /// Статус действия над записью – причина появления записи
    /// </summary>
    [ForeignKey("OperationTypeId")]
    public OperationType OperationType { get; set; }
    /// <summary>
    /// Идентификатор статус действия над записью – причина появления записи
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