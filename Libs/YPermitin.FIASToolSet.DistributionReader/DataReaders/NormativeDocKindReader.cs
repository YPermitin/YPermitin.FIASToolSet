using System.Collections;
using System.Xml;
using YPermitin.FIASToolSet.DistributionReader.Models;

namespace YPermitin.FIASToolSet.DistributionReader.DataReaders;

public class NormativeDocKindReader : IEnumerable<NormativeDocKind>
{
    private readonly string _dataFilePath;
    private NormativeDocKindEnumerator _enumerator;
    
    public NormativeDocKindReader(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }
    
    public IEnumerator<NormativeDocKind> GetEnumerator()
    {
        if (_enumerator == null)
        {
            _enumerator = new NormativeDocKindEnumerator(_dataFilePath);
        }
        
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public class NormativeDocKindEnumerator : IEnumerator<NormativeDocKind>
    {
        private readonly string _dataFilePath;

        private NormativeDocKind _current;
        
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

        public NormativeDocKindEnumerator(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public NormativeDocKind Current => _current;

        object IEnumerator.Current => Current;
        
        public bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "NDOCKIND")
                {
                    var idValue = Reader.GetAttribute("ID");
                    var nameValue = Reader.GetAttribute("NAME");

                    if(int.TryParse(idValue, out int id))
                    {
                        var newObject = new NormativeDocKind(id, nameValue);
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