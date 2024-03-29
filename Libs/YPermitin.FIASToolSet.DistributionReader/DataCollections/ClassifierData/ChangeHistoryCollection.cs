using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class ChangeHistoryCollection : FIASObjectCollection<ChangeHistory, ChangeHistoryCollection.ChangeHistoryEnumerator>
{
    public ChangeHistoryCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public override long CalculateCollectionSize()
    {
        long collectionSize = 0;

        using (var reader = XmlReader.Create(_dataFilePath))
        {
            while (reader.Read())
            {
                if (reader.Name == "ITEM")
                    collectionSize += 1;
            }
        }

        return collectionSize;
    }
    
    public class ChangeHistoryEnumerator : FIASObjectEnumerator<ChangeHistory>
    {
        public ChangeHistoryEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "ITEM")
                {
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var addressObjectGuid = Reader.GetAttributeAsGuid("ADROBJECTID");
                    var operationTypeId = Reader.GetAttributeAsInt("OPERTYPEID");
                    var normativeDocId = Reader.GetAttributeAsInt("NDOCID");
                    var changeDate = Reader.GetAttributeAsDateOnly("CHANGEDATE");
                    
                    var newObject = new ChangeHistory(
                        changeId: changeId,
                        objectId: objectId,
                        addressObjectGuid: addressObjectGuid,
                        operationTypeId: operationTypeId,
                        normativeDocId: normativeDocId,
                        changeDate: changeDate);
                    
                    _current = newObject;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }
}