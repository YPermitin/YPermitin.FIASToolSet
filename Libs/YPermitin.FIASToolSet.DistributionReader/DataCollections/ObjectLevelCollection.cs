using System.Collections;
using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataReaders;

public class ObjectLevelCollection : IEnumerable<ObjectLevel>
{
    private readonly string _dataFilePath;
    private ObjectLevelEnumerator _enumerator;
    
    public ObjectLevelCollection(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }
    
    public IEnumerator<ObjectLevel> GetEnumerator()
    {
        if (_enumerator == null)
        {
            _enumerator = new ObjectLevelEnumerator(_dataFilePath);
        }
        
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public class ObjectLevelEnumerator : IEnumerator<ObjectLevel>
    {
        private readonly string _dataFilePath;

        private ObjectLevel _current;
        
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

        public ObjectLevelEnumerator(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public ObjectLevel Current => _current;

        object IEnumerator.Current => Current;
        
        public bool MoveNext()
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