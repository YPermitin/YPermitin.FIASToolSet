using System.Collections;
using System.Xml;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections;

public abstract class FIASObjectCollection<TItem, TEnumerator> : IEnumerable<TItem> 
    where TItem : class
    where TEnumerator : IEnumerator<TItem>
{
    protected readonly string _dataFilePath;

    public string DataFileFullPath => _dataFilePath;

    public string DataFileShortPath
    {
        get
        {
            string dataFileShortPath = null;
            
            FileInfo dataFileInfo = new FileInfo(_dataFilePath);
            
            // Проверяем, что файл находится в каталоге с данными региона.
            if (dataFileInfo.Directory != null)
            {
                string parentDirectoryName = dataFileInfo.Directory.Name;
                if (parentDirectoryName.Length == 2 && int.TryParse(parentDirectoryName, out _))
                {
                    // При этом дополнительно проверяем,
                    // что родительский каталог содержит файл с информацией о версии ФИАС,
                    // что подтверждает корректную иерархию каталогов.
                    if (dataFileInfo.Directory.Parent != null)
                    {
                        string versionFile = Path.Combine(dataFileInfo.Directory.Parent.FullName, "version.txt");
                        if (File.Exists(versionFile))
                        {
                            dataFileShortPath = Path.Combine(dataFileInfo.Directory.Name, dataFileInfo.Name);
                        }
                    }
                }
            }

            if (dataFileShortPath == null)
            {
                dataFileShortPath = dataFileInfo.Name;
            }

            return dataFileShortPath;
        }
    }
    
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