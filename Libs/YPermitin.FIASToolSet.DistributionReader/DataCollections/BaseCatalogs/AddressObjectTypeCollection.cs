using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

public class AddressObjectTypeCollection : FIASObjectCollection<AddressObjectType, AddressObjectTypeCollection.AddressObjectTypeEnumerator>
{
    public AddressObjectTypeCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class AddressObjectTypeEnumerator : FIASObjectEnumerator<AddressObjectType>
    {
        public AddressObjectTypeEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "ADDRESSOBJECTTYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var levelValue = Reader.GetAttribute("LEVEL");
                    var nameValue = Reader.GetAttribute("NAME");
                    var shortNameValue = Reader.GetAttribute("SHORTNAME");
                    var descriptionValue = Reader.GetAttribute("DESC");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");
                    var updateDateValue = Reader.GetAttribute("UPDATEDATE");

                    if(int.TryParse(idValue, out int id)
                       && int.TryParse(levelValue, out int level)
                       && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate)
                       && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate)
                       && DateOnly.TryParse(updateDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly updateDate))
                    {
                        var newObject = new AddressObjectType(id, level, nameValue, shortNameValue, descriptionValue, startDate, endDate, updateDate);
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