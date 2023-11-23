using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

public class HouseTypeCollection : FIASObjectCollection<HouseType, HouseTypeCollection.HouseTypeEnumerator>
{
    public HouseTypeCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public override long CalculateCollectionSize()
    {
        long collectionSize = 0;

        using (var reader = XmlReader.Create(_dataFilePath))
        {
            while (reader.Read())
            {
                if (reader.Name == "HOUSETYPE")
                    collectionSize += 1;
            }
        }

        return collectionSize;
    }
    
    public class HouseTypeEnumerator : FIASObjectEnumerator<HouseType>
    {
        public HouseTypeEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "HOUSETYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");
                    var shortNameValue = Reader.GetAttribute("SHORTNAME");
                    var descriptionValue = Reader.GetAttribute("DESC");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");
                    var updateDateValue = Reader.GetAttribute("UPDATEDATE");
                    var isActiveValue = Reader.GetAttribute("ISACTIVE");

                    if (int.TryParse(idValue, out int id)
                        && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None,
                            out DateOnly startDate)
                        && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None,
                            out DateOnly endDate)
                        && DateOnly.TryParse(updateDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None,
                            out DateOnly updateDate)
                        && bool.TryParse(isActiveValue, out bool isActive))
                    {
                        _current = new HouseType(id, nameValue, shortNameValue, descriptionValue, 
                            startDate, endDate, updateDate, isActive);
                        return true;
                    }
                }
            }

            _current = null;
            return false;
        }
    }
}