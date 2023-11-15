using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Реестр адресных элементов
/// </summary>
public class ObjectRegistry
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
    /// Признак действующего объекта
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Уровень адресного объекта
    /// </summary>
    [ForeignKey("LevelId")]
    public ObjectLevel Level { get; set; }
    /// <summary>
    /// Идентификатор уровеня адресного объекта
    /// </summary>
    public int LevelId { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreateDate { get; set; }
    
    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTime UpdateDate { get; set; }
    
    /// <summary>
    /// Объект ключа элемента реестра адресных элементов
    /// </summary>
    public class  ObjectRegistryItemKey
    {
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
    }
}