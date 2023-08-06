using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

public class ApartmentTypeCollection : FIASObjectCollection<ApartmentType, ApartmentTypeCollection.ApartmentTypeEnumerator>
{
    public ApartmentTypeCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class ApartmentTypeEnumerator : FIASObjectEnumerator<ApartmentType>
    {
        public ApartmentTypeEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "APARTMENTTYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");
                    var shortNameValue = Reader.GetAttribute("SHORTNAME");
                    var descriptionValue = Reader.GetAttribute("DESC");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");
                    var updateDateValue = Reader.GetAttribute("UPDATEDATE");
                    var isActiveValue = Reader.GetAttribute("ISACTIVE");

                    if(int.TryParse(idValue, out int id)
                       && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate)
                       && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate)
                       && DateOnly.TryParse(updateDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly updateDate)
                       && bool.TryParse(isActiveValue, out bool isActive))
                    {
                        _current = new ApartmentType(id, nameValue, shortNameValue, descriptionValue, startDate, endDate, updateDate, isActive);
                        return true;
                    }
                }
            }

            _current = null;
            return false;
        }
    }
}