using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections;

public class OperationTypeCollection : FIASObjectCollection<OperationType, OperationTypeCollection.OperationTypeEnumerator>
{
    public OperationTypeCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class OperationTypeEnumerator : FIASObjectEnumerator<OperationType>
    {
        public OperationTypeEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "OPERATIONTYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");
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
                        var newObject = new OperationType(id, nameValue, startDate, endDate, updateDate, isActive);
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