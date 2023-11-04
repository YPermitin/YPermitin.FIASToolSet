using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class AddressObjectAdmHierarchyCollection : FIASObjectCollection<AddressObjectAdmHierarchy, AddressObjectAdmHierarchyCollection.AddressObjectAdmHierarchyEnumerator>
{
    public AddressObjectAdmHierarchyCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class AddressObjectAdmHierarchyEnumerator : FIASObjectEnumerator<AddressObjectAdmHierarchy>
    {
        public AddressObjectAdmHierarchyEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "ITEM")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var parentObjectId = Reader.GetAttributeAsInt("PARENTOBJID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var regionCode = Reader.GetAttributeAsInt("REGIONCODE");
                    var areaCode = Reader.GetAttributeAsInt("AREACODE");
                    var cityCode = Reader.GetAttributeAsInt("CITYCODE");
                    var placeCode = Reader.GetAttributeAsInt("PLACECODE");
                    var planCode = Reader.GetAttributeAsInt("PLANCODE");
                    var streetCode = Reader.GetAttributeAsInt("STREETCODE");
                    var prevId = Reader.GetAttributeAsInt("PREVID");
                    var nextId = Reader.GetAttributeAsInt("NEXTID");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    var path = Reader.GetAttributeAsString("PATH");
                    
                    var newObject = new AddressObjectAdmHierarchy(
                        id: id,
                        objectId: objectId,
                        parentObjectId: parentObjectId,
                        changeId: changeId,
                        regionCode: regionCode,
                        areaCode: areaCode,
                        cityCode: cityCode,
                        placeCode: placeCode,
                        planCode: planCode,
                        streetCode: streetCode,
                        previousAddressObjectId: prevId,
                        nextAddressObjectId: nextId,
                        isActive: isActive,
                        path: path,
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