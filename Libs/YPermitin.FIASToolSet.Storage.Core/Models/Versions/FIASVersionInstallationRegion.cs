using System.ComponentModel.DataAnnotations.Schema;

namespace YPermitin.FIASToolSet.Storage.Core.Models.Versions;

/// <summary>
/// Информация о состоянии установки / обновления региона дистрибутива ФИАС
/// </summary>
public class FIASVersionInstallationRegion
{
    /// <summary>
    /// Идентификатор шага
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Установка версия ФИАС
    /// </summary>
    [ForeignKey("FIASVersionInstallationId")]
    public FIASVersionInstallation FIASVersionInstallation { get; set; }
    /// <summary>
    /// Идентификатор установки версии ФИАС
    /// </summary>        
    public Guid FIASVersionInstallationId { get; set; }
    
    /// <summary>
    /// Статус установки данных региона дистрибутива ФИАС
    /// </summary>
    [ForeignKey("StatusId")]
    public FIASVersionInstallationStatus Status { get; set; }
    /// <summary>
    /// Идентификатор статуса установки данных региона дистрибутива ФИАС
    /// </summary>        
    public Guid StatusId { get; set; }
    
    /// <summary>
    /// Код региона
    /// </summary>
    public int RegionCode { get; set; }
}