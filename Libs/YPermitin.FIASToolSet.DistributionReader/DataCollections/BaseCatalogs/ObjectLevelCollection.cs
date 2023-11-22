using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

public class ObjectLevelCollection : FIASObjectCollection<ObjectLevel, ObjectLevelCollection.ObjectLevelEnumerator>
{
    public ObjectLevelCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public override long CalculateCollectionSize()
    {
        long collectionSize = 0;

        using (var reader = XmlReader.Create(_dataFilePath))
        {
            while (reader.Read())
            {
                if (reader.Name == "OBJECTLEVEL")
                    collectionSize += 1;
            }
        }

        return collectionSize;
    }
    
    public class ObjectLevelEnumerator : FIASObjectEnumerator<ObjectLevel>
    {
        public ObjectLevelEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "OBJECTLEVEL")
                {
                    var levelValue = Reader.GetAttribute("LEVEL");
                    var nameValue = Reader.GetAttribute("NAME");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");
                    var updateDateValue = Reader.GetAttribute("UPDATEDATE");
                    var isActiveValue = Reader.GetAttribute("ISACTIVE");

                    if(int.TryParse(levelValue, out int level)
                       && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate)
                       && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate)
                       && DateOnly.TryParse(updateDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly updateDate)
                       && bool.TryParse(isActiveValue, out bool isActive))
                    {
                        var newObject = new ObjectLevel(level, nameValue, startDate, endDate, updateDate, isActive);
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