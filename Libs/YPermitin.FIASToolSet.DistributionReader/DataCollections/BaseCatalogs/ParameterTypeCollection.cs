using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

public class ParameterTypeCollection : FIASObjectCollection<ParameterType, ParameterTypeCollection.ParameterTypeEnumerator>
{
    public ParameterTypeCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class ParameterTypeEnumerator : FIASObjectEnumerator<ParameterType>
    {
        public ParameterTypeEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "PARAMTYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");
                    var descriptionValue = Reader.GetAttribute("DESC");
                    var codeValue = Reader.GetAttribute("CODE");
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
                        var newObject = new ParameterType(id, nameValue, descriptionValue, codeValue, startDate, endDate, updateDate, isActive);
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