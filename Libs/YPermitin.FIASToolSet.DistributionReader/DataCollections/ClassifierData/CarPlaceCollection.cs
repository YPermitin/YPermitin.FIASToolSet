using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class CarPlaceCollection : FIASObjectCollection<CarPlace, CarPlaceCollection.CarPlaceEnumerator>
{
    public CarPlaceCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public override long CalculateCollectionSize()
    {
        long collectionSize = 0;

        using (var reader = XmlReader.Create(_dataFilePath))
        {
            while (reader.Read())
            {
                if (reader.Name == "CARPLACE")
                    collectionSize += 1;
            }
        }

        return collectionSize;
    }
    
    public class CarPlaceEnumerator : FIASObjectEnumerator<CarPlace>
    {
        public CarPlaceEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "CARPLACE")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var objectGuid = Reader.GetAttributeAsGuid("OBJECTGUID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var number = Reader.GetAttributeAsString("NUMBER");
                    var operationTypeId = Reader.GetAttributeAsInt("OPERTYPEID");
                    var prevId = Reader.GetAttributeAsInt("PREVID");
                    var nextId = Reader.GetAttributeAsInt("NEXTID");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    var isActual = Reader.GetAttributeAsBool("ISACTUAL");
                    
                    var newObject = new CarPlace(
                        id: id,
                        objectId: objectId,
                        objectGuid: objectGuid,
                        changeId: changeId,
                        number: number,
                        operationTypeId: operationTypeId,
                        previousAddressObjectId: prevId,
                        nextAddressObjectId: nextId,
                        isActive: isActive,
                        isActual: isActual,
                        updateDate: updateDate,
                        startDate: startDate,
                        endDate: endDate);
                    
                    _current = newObject;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }
}