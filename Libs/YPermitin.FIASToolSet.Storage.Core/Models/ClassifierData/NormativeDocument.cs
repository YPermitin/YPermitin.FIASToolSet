using System.ComponentModel.DataAnnotations.Schema;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

/// <summary>
/// Нормативный документ
/// </summary>
public class NormativeDocument
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование документа
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Дата документа
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Номер документа
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Тип документа
    /// </summary>
    [ForeignKey("TypeId")]
    public NormativeDocType NormativeDocType { get; set; }
    /// <summary>
    /// Идентификатор типа документа
    /// </summary>
    public int TypeId { get; set; }
    
    /// <summary>
    /// Вид документа
    /// </summary>
    [ForeignKey("KindId")]
    public NormativeDocKind NormativeDocKind { get; set; }
    /// <summary>
    /// Идентификатор вида документа
    /// </summary>
    public int KindId { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// Наименование органа, создавшего нормативный документ
    /// </summary>
    public string OrgName { get; set; }

    /// <summary>
    /// Номер государственной регистрации
    /// </summary>
    public string RegNumber { get; set; }
    
    /// <summary>
    /// Дата государственной регистрации
    /// </summary>
    public DateTime RegDate { get; set; }
    
    /// <summary>
    /// Дата вступления в силу нормативного документа
    /// </summary>
    public DateTime AccDate { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; }
}