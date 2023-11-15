using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
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
                    var id = Reader.GetAttributeAsInt("ID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var objectGuid = Reader.GetAttributeAsGuid("OBJECTGUID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var name = Reader.GetAttributeAsString("NAME");
                    var typeName = Reader.GetAttribute("TYPENAME");
                    var levelId = Reader.GetAttributeAsInt("LEVEL");
                    var operationTypeId = Reader.GetAttributeAsInt("OPERTYPEID");
                    var previousAddressObjectId = Reader.GetAttributeAsInt("PREVID");
                    var nextAddressObjectId = Reader.GetAttributeAsInt("NEXTID");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    var isActual = Reader.GetAttributeAsBool("ISACTUAL");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    
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
                        isActive: isActive,
                        isActual: isActual
                    );
                        
                    _current = newObject;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }
}