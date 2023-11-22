using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class RoomParameterCollection : FIASObjectCollection<RoomParameter, RoomParameterCollection.RoomParameterEnumerator>
{
    public RoomParameterCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public override long CalculateCollectionSize()
    {
        long collectionSize = 0;

        using (var reader = XmlReader.Create(_dataFilePath))
        {
            while (reader.Read())
            {
                if (reader.Name == "PARAM")
                    collectionSize += 1;
            }
        }

        return collectionSize;
    }
    
    public class RoomParameterEnumerator : FIASObjectEnumerator<RoomParameter>
    {
        public RoomParameterEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "PARAM")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var changeIdEnd = Reader.GetAttributeAsInt("CHANGEIDEND");
                    var typeId = Reader.GetAttributeAsInt("TYPEID");
                    var sourceValue = Reader.GetAttributeAsString("VALUE");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    
                    var newObject = new RoomParameter(
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

            _current = null;
            return false;
        }
    }
}