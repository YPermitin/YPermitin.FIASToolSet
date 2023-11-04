namespace YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

/// <summary>
/// Параметры комнаты
/// </summary>
public class RoomParameter : ClassifierDataParameter
{
    public RoomParameter(
        int id, 
        int objectId, 
        int changeId, 
        int changeIdEnd, 
        int typeId, 
        string value, 
        DateOnly updateDate, 
        DateOnly startDate,
        DateOnly endDate) 
        : base(id, objectId, changeId, changeIdEnd, typeId, value, updateDate, startDate, endDate)
    {
    }
}