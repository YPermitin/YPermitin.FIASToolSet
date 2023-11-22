using System.Collections;
using System.Xml;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections;

public abstract class FIASObjectCollection<TItem, TEnumerator> : IEnumerable<TItem> 
    where TItem : class
    where TEnumerator : IEnumerator<TItem>
{
    protected readonly string _dataFilePath;
    
    private IEnumerator<TItem> _enumerator;

    protected FIASObjectCollection(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }
    
    public IEnumerator<TItem> GetEnumerator()
    {
        if (_enumerator == null)
        {
            _enumerator = (IEnumerator<TItem>)Activator.CreateInstance(typeof(TEnumerator), _dataFilePath);
        }
        
        return _enumerator;
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual long CalculateCollectionSize()
    {
        return this.LongCount();
    }
    
    public abstract class FIASObjectEnumerator<TParsedItem> : IEnumerator<TParsedItem> where TParsedItem : class
    {
        private readonly string _dataFilePath;

        protected TParsedItem _current;
        
        private XmlReader _reader;
        protected XmlReader Reader {
            get
            {
                if (_reader == null)
                {
                    _reader = XmlReader.Create(_dataFilePath);
                }
                return _reader;
            }
        }

        protected FIASObjectEnumerator(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }
        
        public TParsedItem Current => _current;

        object IEnumerator.Current => Current;


        bool IEnumerator.MoveNext()
        {
            return MoveNext();
        }

        protected abstract bool MoveNext();
        
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