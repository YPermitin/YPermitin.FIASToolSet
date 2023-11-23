using System.ComponentModel.DataAnnotations.Schema;

namespace YPermitin.FIASToolSet.Storage.Core.Models.Versions;

/// <summary>
/// Информация о шаге установки / обновления дистрибутива ФИАС
/// </summary>
public class FIASVersionInstallationStep
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
    /// Путь файла дистрибутива ФИАС в каталоге с дистрибутивом.
    ///
    /// Указан относительно корневого каталога дистрибутива ФИАС.
    /// </summary>
    public string FileFullName { get; set; }
    
    /// <summary>
    /// Статус установки файла дистрибутива ФИАС
    /// </summary>
    [ForeignKey("StatusId")]
    public FIASVersionInstallationStatus Status { get; set; }
    /// <summary>
    /// Идентификатор статуса установки файла дистрибутива ФИАС
    /// </summary>        
    public Guid StatusId { get; set; }
    
    /// <summary>
    /// Общее количество элементов в файле для загрузки.
    /// </summary>
    public long TotalItemsToLoad { get; set; }
    
    /// <summary>
    /// Общее количество элементов, загруженных из файла.
    /// </summary>
    public long TotalItemsLoaded { get; set; }
    
    /// <summary>
    /// % выполнения загрузки данных из файла.
    /// </summary>
    public int PercentageCompleted { get; set; }
    
    /// <summary>
    /// Начало обработки шага
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Окончание обработки шага
    /// </summary>
    public DateTime EndDate { get; set; }
}