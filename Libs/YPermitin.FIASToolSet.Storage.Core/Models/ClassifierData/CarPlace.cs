using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Машино-место
/// </summary>
public class CarPlace
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Глобальный уникальный идентификатор объекта
    /// </summary>
    public int ObjectId { get; set; }
    
    /// <summary>
    /// Глобальный уникальный идентификатор объекта
    /// типа UUID
    /// </summary>
    public Guid ObjectGuid { get; set; }
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public int ChangeId { get; set; }
    
    /// <summary>
    /// Номер машино-места
    /// </summary>
    public string Number { get; set; }
    
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
    /// Статус актуальности объекта ФИАС
    /// </summary>
    public bool IsActual { get; set; }
    
    /// <summary>
    /// Признак действующего объекта
    /// </summary>
    public bool IsActive { get; set; }
}