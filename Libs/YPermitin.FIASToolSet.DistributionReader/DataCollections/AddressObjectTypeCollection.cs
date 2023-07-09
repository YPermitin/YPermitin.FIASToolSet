using System.Collections;
using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataReaders;

public class AddressObjectTypeCollection : IEnumerable<AddressObjectType>
{
    private readonly string _dataFilePath;
    private AddressObjectTypeEnumerator _enumerator;
    
    public AddressObjectTypeCollection(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }
    
    public IEnumerator<AddressObjectType> GetEnumerator()
    {
        if (_enumerator == null)
        {
            _enumerator = new AddressObjectTypeEnumerator(_dataFilePath);
        }
        
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public class AddressObjectTypeEnumerator : IEnumerator<AddressObjectType>
    {
        private readonly string _dataFilePath;

        private AddressObjectType _current;
        
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

        public AddressObjectTypeEnumerator(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public AddressObjectType Current => _current;

        object IEnumerator.Current => Current;
        
        public bool MoveNext()
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