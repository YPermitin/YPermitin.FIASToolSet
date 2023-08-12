using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;
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
                    var idValue = Reader.GetAttribute("ID");
                    var parentIdValue = Reader.GetAttribute("PARENTID");
                    var childIdValue = Reader.GetAttribute("CHILDID");
                    var changeIdValue = Reader.GetAttribute("CHANGEID");
                    
                    if(int.TryParse(idValue, out int id)
                       && int.TryParse(parentIdValue, out int parentId)
                       && int.TryParse(childIdValue, out int childId)
                       && int.TryParse(changeIdValue, out int changeId))
                    {
                        var newObject = new AddressObjectDivision(
                            id: id,
                            parentId: parentId,
                            childId: childId,
                            changeId: changeId);
                        _current = newObject;
                        return true;
                    }
                }
            }

            _current = null;
            return false;
        }
    }
}