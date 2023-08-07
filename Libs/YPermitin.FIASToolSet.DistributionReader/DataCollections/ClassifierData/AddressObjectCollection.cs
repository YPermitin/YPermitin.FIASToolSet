using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class AddressObjectCollection : FIASObjectCollection<AddressObject, AddressObjectCollection.AddressObjectEnumerator>
{
    public AddressObjectCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class AddressObjectEnumerator : FIASObjectEnumerator<AddressObject>
    {
        public AddressObjectEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "OBJECT")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var objectIdValue = Reader.GetAttribute("OBJECTID");
                    var objectGuidValue = Reader.GetAttribute("OBJECTGUID");
                    var changeIdValue = Reader.GetAttribute("CHANGEID");
                    var name = Reader.GetAttribute("NAME");
                    var typeName = Reader.GetAttribute("TYPENAME");
                    var levelIdValue = Reader.GetAttribute("LEVEL");
                    var operationTypeIdValue = Reader.GetAttribute("OPERTYPEID");
                    var previousAddressObjectIdValue = Reader.GetAttribute("PREVID");
                    var nextAddressObjectIdValue = Reader.GetAttribute("NEXTID");
                    var updateDateValue = Reader.GetAttribute("UPDATEDATE");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");
                    var isActualValue = Reader.GetAttribute("ISACTUAL");
                    var isActiveValue = Reader.GetAttribute("ISACTIVE");
                    
                    if(int.TryParse(idValue, out int id)
                       && int.TryParse(objectIdValue, out int objectId)
                       && Guid.TryParse(objectGuidValue, out Guid objectGuid)
                       && int.TryParse(changeIdValue, out int changeId)
                       && int.TryParse(levelIdValue, out int levelId)
                       && int.TryParse(operationTypeIdValue, out int operationTypeId)
                       && int.TryParse(previousAddressObjectIdValue, out int previousAddressObjectId)
                       && int.TryParse(nextAddressObjectIdValue, out int nextAddressObjectId)
                       && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate)
                       && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate)
                       && DateOnly.TryParse(updateDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly updateDate)
                       && int.TryParse(isActiveValue, out int isActive)
                       && int.TryParse(isActualValue, out int isActual))
                    {
                        var newObject = new AddressObject(
                            id: id,
                            objectId: objectId,
                            objectGuid: objectGuid,
                            changeId: changeId,
                            name: name,
                            typeName: typeName,
                            levelId: levelId,
                            operationTypeId: operationTypeId,
                            previousAddressObjectId: previousAddressObjectId,
                            nextAddressObjectId: nextAddressObjectId,
                            updateDate: updateDate,
                            startDate: startDate,
                            endDate: endDate,
                            isActive: (isActive == 1),
                            isActual: (isActual == 1)
                            );
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