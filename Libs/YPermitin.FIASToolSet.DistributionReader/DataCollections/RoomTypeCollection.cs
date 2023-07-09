using System.Collections;
using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataReaders;

public class RoomTypeCollection : IEnumerable<RoomType>
{
    private readonly string _dataFilePath;
    private RoomTypeEnumerator _enumerator;
    
    public RoomTypeCollection(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }
    
    public IEnumerator<RoomType> GetEnumerator()
    {
        if (_enumerator == null)
        {
            _enumerator = new RoomTypeEnumerator(_dataFilePath);
        }
        
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public class RoomTypeEnumerator : IEnumerator<RoomType>
    {
        private readonly string _dataFilePath;

        private RoomType _current;
        
        private XmlReader _reader;
        private XmlReader Reader {
            get
            {
                if (_reader == null)
                {
                    _reader = XmlReader.Create(_dataFilePath);
                }
                return _reader;
            }
        }

        public RoomTypeEnumerator(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public RoomType Current => _current;

        object IEnumerator.Current => Current;
        
        public bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "ROOMTYPE")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");
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
                        var newObject = new RoomType(id, nameValue, descriptionValue, startDate, endDate, updateDate, isActive);
                        _current = newObject;
                        return true;
                    }
                    else
                    {
                        _current = null;
                    }
                }
            }

            DisposeXmlReader();
            return false;
        }

        public void Reset()
        {
            DisposeXmlReader();
        }

        public void Dispose()
        {
            DisposeXmlReader();
        }

        private void DisposeXmlReader()
        {
            if (_reader != null)
            {
                if(_reader.ReadState != ReadState.Closed)
                    _reader.Close();
                
                _reader.Dispose();

                _reader = null;
            }
        }
    }
}