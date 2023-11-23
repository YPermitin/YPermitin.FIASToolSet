using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class HouseCollection : FIASObjectCollection<House, HouseCollection.HouseEnumerator>
{
    public HouseCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public override long CalculateCollectionSize()
    {
        long collectionSize = 0;

        using (var reader = XmlReader.Create(_dataFilePath))
        {
            while (reader.Read())
            {
                if (reader.Name == "HOUSE")
                    collectionSize += 1;
            }
        }

        return collectionSize;
    }
    
    public class HouseEnumerator : FIASObjectEnumerator<House>
    {
        public HouseEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "HOUSE")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var objectGuid = Reader.GetAttributeAsGuid("OBJECTGUID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var houseNumber = Reader.GetAttributeAsString("HOUSENUM");
                    var addHouseNumber1 = Reader.GetAttributeAsString("ADDNUM1");
                    var addHouseNumber2 = Reader.GetAttributeAsString("ADDNUM2");
                    var houseTypeId = Reader.GetAttributeAsInt("HOUSETYPE");
                    var addHouseTypeId1 = Reader.GetAttributeAsInt("ADDTYPE1");
                    var addHouseTypeId2 = Reader.GetAttributeAsInt("ADDTYPE2");
                    var operationTypeId = Reader.GetAttributeAsInt("OPERTYPEID");
                    var prevId = Reader.GetAttributeAsInt("PREVID");
                    var nextId = Reader.GetAttributeAsInt("NEXTID");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    var isActual = Reader.GetAttributeAsBool("ISACTUAL");
                    
                    var newObject = new House(
                        id: id,
                        objectId: objectId,
                        objectGuid: objectGuid,
                        changeId: changeId,
                        houseNumber: houseNumber,
                        addedHouseNumber1: addHouseNumber1,
                        addedHouseNumber2: addHouseNumber2,
                        houseTypeId: houseTypeId,
                        addedHouseTypeId1: addHouseTypeId1,
                        addedHouseTypeId2: addHouseTypeId2,
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