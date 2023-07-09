using System.Collections;
using System.Globalization;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataReaders;

public class NormativeDocTypeCollection : IEnumerable<NormativeDocType>
{
    private readonly string _dataFilePath;
    private NormativeDocTypeEnumerator _enumerator;
    
    public NormativeDocTypeCollection(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }
    
    public IEnumerator<NormativeDocType> GetEnumerator()
    {
        if (_enumerator == null)
        {
            _enumerator = new NormativeDocTypeEnumerator(_dataFilePath);
        }
        
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public class NormativeDocTypeEnumerator : IEnumerator<NormativeDocType>
    {
        private readonly string _dataFilePath;

        private NormativeDocType _current;
        
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

        public NormativeDocTypeEnumerator(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public NormativeDocType Current => _current;

        object IEnumerator.Current => Current;
        
        public bool MoveNext()
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