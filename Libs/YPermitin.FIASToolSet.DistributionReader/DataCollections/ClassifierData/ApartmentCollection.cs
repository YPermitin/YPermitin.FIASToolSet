using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class ApartmentCollection : FIASObjectCollection<Apartment, ApartmentCollection.ApartmentEnumerator>
{
    public ApartmentCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class ApartmentEnumerator : FIASObjectEnumerator<Apartment>
    {
        public ApartmentEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "APARTMENT")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var objectGuid = Reader.GetAttributeAsGuid("OBJECTGUID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var numberValue = Reader.GetAttribute("NUMBER");
                    var apartmentTypeId = Reader.GetAttributeAsInt("APARTTYPE");
                    var operationTypeId = Reader.GetAttributeAsInt("OPERTYPEID");
                    var prevId = Reader.GetAttributeAsInt("PREVID");
                    var nextId = Reader.GetAttributeAsInt("NEXTID");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    var isActual = Reader.GetAttributeAsBool("ISACTUAL");
                    
                    var newObject = new Apartment(
                        id: id,
                        objectId: objectId,
                        objectGuid: objectGuid,
                        changeId: changeId,
                        number: numberValue,
                        apartmentTypeId: apartmentTypeId,
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