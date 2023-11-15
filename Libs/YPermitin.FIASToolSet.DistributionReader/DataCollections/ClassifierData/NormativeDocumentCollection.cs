using System.Globalization;
using YPermitin.FIASToolSet.DistributionReader.Extensions;
using YPermitin.FIASToolSet.DistributionReader.Models.ClassifierData;

namespace YPermitin.FIASToolSet.DistributionReader.DataCollections.ClassifierData;

public class NormativeDocumentCollection : FIASObjectCollection<NormativeDocument, NormativeDocumentCollection.NormativeDocumentEnumerator>
{
    public NormativeDocumentCollection(string dataFilePath) : base(dataFilePath)
    {
    }
    
    public class NormativeDocumentEnumerator : FIASObjectEnumerator<NormativeDocument>
    {
        public NormativeDocumentEnumerator(string dataFilePath) : base(dataFilePath)
        {
        }

        protected override bool MoveNext()
        {
            while (Reader.Read())
            {
                if (Reader.Name == "NORMDOC")
                {
                    var id = Reader.GetAttributeAsInt("ID");
                    var name = Reader.GetAttributeAsString("NAME");
                    var date = Reader.GetAttributeAsDateOnly("DATE");
                    var number = Reader.GetAttributeAsString("NUMBER");
                    var typeId = Reader.GetAttributeAsInt("TYPE");
                    var kindId = Reader.GetAttributeAsInt("KIND");
                    var updateDate = Reader.GetAttributeAsDateOnly("UPDATEDATE");
                    var orgName = Reader.GetAttributeAsString("ORGNAME");
                    var regNum = Reader.GetAttributeAsString("REGNUM");
                    var regDate = Reader.GetAttributeAsDateOnly("REGDATE");
                    var accDate = Reader.GetAttributeAsDateOnly("ACCDATE");
                    var comment = Reader.GetAttributeAsString("COMMENT");

                    var newObject = new NormativeDocument(
                        id: id,
                        name: name,
                        date: date,
                        number: number,
                        typeId: typeId,
                        kindId: kindId,
                        updateDate: updateDate,
                        orgName: orgName,
                        regNumber: regNum,
                        regDate: regDate,
                        accDate: accDate,
                        comment: comment);
                    
                    _current = newObject;
                    return true;
                }
            }

            _current = null;
            return false;
        }
    }
}