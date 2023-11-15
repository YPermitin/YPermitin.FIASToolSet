using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class AddressObjectDivisionCollection : FIASObjectCollection<AddressObjectDivision, AddressObjectDivisionCollection.AddressObjectDivisionEnumerator>
{
    public AddressObjectDivisionCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class AddressObjectDivisionEnumerator : FIASObjectEnumerator<AddressObjectDivision>
    {
        public AddressObjectDivisionEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "ITEM")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var parentId = Reader.GetAttributeAsInt("PARENTID");
                    var childId = Reader.GetAttributeAsInt("CHILDID");
                    var changeId = Reader.GetAttributeAsInt("CHANGEID");
                    
                    var newObject = new AddressObjectDivision(
                        id: id,
                        parentId: parentId,
                        childId: childId,
                        changeId: changeId);
                    
                    _current = newObject;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }
}