using System;
using System.IO;
using System.Linq;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class NormativeDocTypeCollectionTests
{
    private readonly string _dataFile;

    public NormativeDocTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_NORMATIVE_DOCS_TYPES_20230706_35b52e02-e36e-4583-bc20-9d1a81c6c4c2.XML"
        );
    }

    [Fact]
    public void ReadItems()
    {
        var collection = new NormativeDocTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(25, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не указан", allItems[0].Name);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2016, 3, 31), allItems[0].EndDate);
    }
}