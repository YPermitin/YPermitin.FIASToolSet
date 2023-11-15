using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Общий параметр адресных объектов
/// </summary>
public abstract class ClassifierDataParameter
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
    /// ID изменившей транзакции
    /// </summary>
    public int ChangeId { get; set; }
    
    /// <summary>
    /// ID завершившей транзакции
    /// </summary>
    public int ChangeIdEnd { get; set; }
    
    /// <summary>
    /// Тип параметра
    /// </summary>
    [ForeignKey("TypeId")]
    public ParameterType ParameterType { get; set; }
    /// <summary>
    /// Идентификатор типа параметра
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Значение параметра
    /// </summary>
    public string Value { get; set; }
    
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
}