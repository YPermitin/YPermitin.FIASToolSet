namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Нормативный документ
/// </summary>
public class NormativeDocument
{
    /// <summary>
    /// Идентификатор записи
    /// </summary>
    public readonly int Id;

    /// <summary>
    /// Наименование документа
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Дата документа
    /// </summary>
    public readonly DateOnly Date;

    /// <summary>
    /// Номер документа
    /// </summary>
    public readonly string Number;

    /// <summary>
    /// Тип документа
    /// </summary>
    public readonly int TypeId;
    
    /// <summary>
    /// Вид документа
    /// </summary>
    public readonly int KindId;

    /// <summary>
    /// Дата обновления
    /// </summary>
    public readonly DateOnly UpdateDate;

    /// <summary>
    /// Наименование органа, создавшего нормативный документ
    /// </summary>
    public readonly string OrgName;

    /// <summary>
    /// Номер государственной регистрации
    /// </summary>
    public readonly string RegNumber;
    
    /// <summary>
    /// Дата государственной регистрации
    /// </summary>
    public readonly DateOnly RegDate;
    
    /// <summary>
    /// Дата вступления в силу нормативного документа
    /// </summary>
    public readonly DateOnly AccDate;

    /// <summary>
    /// Комментарий
    /// </summary>
    public readonly string Comment;

    public NormativeDocument(int id, string name, DateOnly date, string number,
        int typeId, int kindId, DateOnly updateDate, string orgName, 
        string regNumber, DateOnly regDate, DateOnly accDate, string comment)
    {
        Id = id;
        Name = name;
        Date = date;
        Number = number;
        TypeId = typeId;
        KindId = kindId;
        UpdateDate = updateDate;
        OrgName = orgName;
        RegNumber = regNumber;
        RegDate = regDate;
        AccDate = accDate;
        Comment = comment;
    }
}