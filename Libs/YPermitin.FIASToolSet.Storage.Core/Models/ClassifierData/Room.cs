using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Комната
/// </summary>
public class Room
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
    /// Номер комнаты или офиса
    /// </summary>
    public string RoomNumber { get; set; }
    
    /// <summary>
    /// Тип комнаты или офиса
    /// </summary>
    [ForeignKey("RoomTypeId")]
    public RoomType RoomType { get; set; }
    /// <summary>
    /// Идентификатор типа комнаты или офиса
    /// </summary>
    public int RoomTypeId { get; set; }
    
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