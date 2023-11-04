using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class ObjectRegistryCollection : FIASObjectCollection<ObjectRegistry, ObjectRegistryCollection.ObjectRegistryEnumerator>
{
    public ObjectRegistryCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class ObjectRegistryEnumerator : FIASObjectEnumerator<ObjectRegistry>
    {
        public ObjectRegistryEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "OBJECT")
                {
                    var objectId = Reader.GetAttributeAsInt("OBJECTID");
                    var objectGuid = Reader.GetAttributeAsGuid("OBJECTGUID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    var isActive = Reader.GetAttributeAsBool("ISACTIVE");
                    var levelId = Reader.GetAttributeAsInt("LEVELID");
                    var createDate = Reader.GetAttributeAsDateOnly("CREATEDATE");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");

                    var newObject = new ObjectRegistry(
                        objectId: objectId,
                        objectGuid: objectGuid,
                        changeId: changeId,
                        isActive: isActive,
                        levelId: levelId,
                        createDate: createDate,
                        updateDate: updateDate);
                    
                    _current = newObject;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }
}