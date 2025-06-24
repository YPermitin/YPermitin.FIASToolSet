using System.IO;
using System.Linq;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

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

    [Fact]
    public void ReadItems()
    {
        var collection = new NormativeDocKindCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(4, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не определено", allItems[0].Name);
    }
}