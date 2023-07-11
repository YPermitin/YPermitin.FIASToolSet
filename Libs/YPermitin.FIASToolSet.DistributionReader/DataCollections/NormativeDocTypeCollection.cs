using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections;

public class NormativeDocTypeCollection : FIASObjectCollection<NormativeDocType, NormativeDocTypeCollection.NormativeDocTypeEnumerator>
{
    public NormativeDocTypeCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class NormativeDocTypeEnumerator : FIASObjectEnumerator<NormativeDocType>
    {
        public NormativeDocTypeEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "NDOCTYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");
                    var startDateValue = Reader.GetAttribute("STARTDATE");
                    var endDateValue = Reader.GetAttribute("ENDDATE");

                    if(int.TryParse(idValue, out int id)
                       && DateOnly.TryParse(startDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly startDate)
                       && DateOnly.TryParse(endDateValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly endDate))
                    {
                        var newObject = new NormativeDocType(id, nameValue, startDate, endDate);
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