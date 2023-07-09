using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataReaders;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataReaders;

public class NormativeDocKindCollectionTests
{
    private readonly string _dataFile;

    public NormativeDocKindCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_NORMATIVE_DOCS_KINDS_20230706_3ffe3a9e-70c3-44a0-9d94-03ada524851d.XML"
        );
    }

    [Test]
    public void ReadItems()
    {
        var collection = new NormativeDocKindCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(4, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не определено", allItems[0].Name);
    }
}