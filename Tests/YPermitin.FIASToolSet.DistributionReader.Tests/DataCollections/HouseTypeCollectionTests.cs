using System;
using System.IO;
using System.Linq;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class HouseTypeCollectionTests
{
    private readonly string _dataFile;

    public HouseTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_HOUSE_TYPES_20230706_8f22a317-dbec-44e4-9db7-a686a372c1c0.XML"
        );
    }

    [Fact]
    public void ReadItems()
    {
        var collection = new HouseTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(14, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1, allItems[0].Id);
        Assert.Equal("Владение", allItems[0].Name);
        Assert.Equal("влд.", allItems[0].ShortName);
        Assert.Equal("Владение", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(false, allItems[0].IsActive);
    }
}