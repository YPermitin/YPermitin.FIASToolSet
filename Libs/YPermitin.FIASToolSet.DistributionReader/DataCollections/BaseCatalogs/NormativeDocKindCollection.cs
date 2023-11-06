using YPermitin.FIASToolSet.DistributionReader.Models.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

public class NormativeDocKindCollection : FIASObjectCollection<NormativeDocKind, NormativeDocKindCollection.NormativeDocKindEnumerator>
{
    public NormativeDocKindCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class NormativeDocKindEnumerator : FIASObjectEnumerator<NormativeDocKind>
    {
        public NormativeDocKindEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
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
                }
            }

            _current = null;
            return false;
        }
    }
}