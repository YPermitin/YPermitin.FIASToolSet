using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class AddressObjectParameterCollection : FIASObjectCollection<AddressObjectParameter, AddressObjectParameterCollection.AddressObjectParameterEnumerator>
{
    public AddressObjectParameterCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class AddressObjectParameterEnumerator : FIASObjectEnumerator<AddressObjectParameter>
    {
        public AddressObjectParameterEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "PARAM")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var objectIdValue = Reader.GetAttribute("OBJECTID");
                    var changeIdValue = Reader.GetAttribute("CHANGEID");
                    var changeIdEndValue = Reader.GetAttribute("CHANGEIDEND");
                    var typeIdValue = Reader.GetAttribute("TYPEID");
                    var sourceValue = Reader.GetAttribute("VALUE");
                    var updateDateValue = Reader.GetAttribute("UPDATEDATE");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");
                    
                    if(int.TryParse(idValue, out int id)
                       && int.TryParse(objectIdValue, out int objectId)
                       && int.TryParse(changeIdEndValue, out int changeIdEnd)
                       && int.TryParse(changeIdValue, out int changeId)
                       && int.TryParse(typeIdValue, out int typeId)
                       && DateOnly.TryParse(updateDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly updateDate)
                       && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate)
                       && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate))
                    {
                        var newObject = new AddressObjectParameter(
                            id: id,
                            objectId: objectId,
                            changeIdEnd: changeIdEnd,
                            changeId: changeId,
                            typeId: typeId,
                            value: sourceValue,
                            updateDate: updateDate,
                            startDate: startDate,
                            endDate: endDate);
                        _current = newObject;
                        return true;
                    }
                }
            }

            _current = null;
            return false;
        }
    }
}