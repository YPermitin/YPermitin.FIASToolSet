using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// История изменения адресных объектов
/// </summary>
public class ChangeHistory
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public int ChangeId { get; set; }
    
    /// <summary>
    /// Глобальный уникальный идентификатор объекта
    /// </summary>
    public int ObjectId { get; set; }
    
    /// <summary>
    /// Глобальный уникальный идентификатор ID изменившей транзакции
    /// Соответствует полю AOID выгрузки в формате ФИАС
    /// </summary>
    public Guid AddressObjectGuid { get; set; }
    
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
    /// Нормативный документ
    /// </summary>
    [ForeignKey("NormativeDocId")]
    public NormativeDocument NormativeDocument { get; set; }
    /// <summary>
    /// Идентификатор нормативного документа
    /// </summary>
    public int? NormativeDocId { get; set; }

    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTime ChangeDate { get; set; }
}