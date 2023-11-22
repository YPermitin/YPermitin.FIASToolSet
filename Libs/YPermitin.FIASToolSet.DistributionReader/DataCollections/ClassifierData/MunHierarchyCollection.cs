using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class MunHierarchyCollection : FIASObjectCollection<MunHierarchy, MunHierarchyCollection.MunHierarchyEnumerator>
{
    public MunHierarchyCollection(string dataFilePath) : base(dataFilePath)
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
    
    public class MunHierarchyEnumerator : FIASObjectEnumerator<MunHierarchy>
    {
        public MunHierarchyEnumerator(string dataFilePath) : base(dataFilePath)
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
                    var oktmo = Reader.GetAttributeAsString("OKTMO");
                    var prevId = Reader.GetAttributeAsInt("PREVID");
                    var nextId = Reader.GetAttributeAsInt("NEXTID");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var startDate = Reader.GetAttributeAsDateOnly("STARTDATE");
                    var endDate = Reader.GetAttributeAsDateOnly("ENDDATE");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    var path = Reader.GetAttributeAsString("PATH");
                    
                    var newObject = new MunHierarchy(
                        id: id,
                        objectId: objectId,
                        parentObjectId: parentObjectId,
                        changeId: changeId,
                        oktmo: oktmo,
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