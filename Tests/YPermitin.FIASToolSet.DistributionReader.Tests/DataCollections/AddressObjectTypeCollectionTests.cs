using System;
using System.IO;
using System.Linq;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class AddressObjectTypeCollectionTests
{
    private readonly string _dataFile;

    public AddressObjectTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_ADDR_OBJ_TYPES_20230706_42f95bf4-bebe-4e6a-ac02-00ab1f652505.XML"
        );
    }

    [Fact]
    public void ReadItems()
    {
        var collection = new AddressObjectTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(421, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(5, allItems[0].Id);
        Assert.Equal(1, allItems[0].Level);
        Assert.Equal("Автономная область", allItems[0].Name);
        Assert.Equal("Аобл", allItems[0].ShortName);
        Assert.Equal("Автономная область", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
    }
}