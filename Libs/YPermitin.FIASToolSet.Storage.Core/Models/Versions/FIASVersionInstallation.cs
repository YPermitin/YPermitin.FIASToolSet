using System.ComponentModel.DataAnnotations.Schema;

namespace YPermitin.FIASToolSet.Storage.Core.Models.Versions;

/// <summary>
/// Информация об установленной версии ФИАС
/// </summary>
public class FIASVersionInstallation
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Период создания
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// Версия ФИАС
    /// </summary>
    [ForeignKey("FIASVersionId")]
    public FIASVersion FIASVersion { get; set; }

    /// <summary>
    /// Идентификатор версии ФИАС
    /// </summary>        
    public Guid FIASVersionId { get; set; }
    
    /// <summary>
    /// Статус установки версии ФИАС
    /// </summary>
    [ForeignKey("StatusId")]
    public FIASVersionInstallationStatus Status { get; set; }

    /// <summary>
    /// Идентификатор статуса установки версии ФИАС
    /// </summary>        
    public Guid StatusId { get; set; }
    
    /// <summary>
    /// Тип установки ФИАС
    /// </summary>
    [ForeignKey("InstallationTypeId")]
    public FIASVersionInstallationType InstallationType { get; set; }

    /// <summary>
    /// Идентификатор типа установки ФИАС
    /// </summary>        
    public Guid InstallationTypeId { get; set; }
    
    /// <summary>
    /// Дата начала установки
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Дата начала установки
    /// </summary>
    public DateTime? FinishDate { get; set; }
}