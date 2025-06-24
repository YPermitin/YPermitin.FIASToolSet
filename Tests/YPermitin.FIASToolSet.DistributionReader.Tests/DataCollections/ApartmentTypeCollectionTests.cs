using System;
using System.IO;
using System.Linq;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class ApartmentTypeCollectionTests
{
    private readonly string _dataFile;

    public ApartmentTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_APARTMENT_TYPES_20230706_e4450931-ca64-4b45-b04d-65471519ddc8.XML"
        );
    }

    [Fact]
    public void ReadItems()
    {
        var collection = new ApartmentTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(13, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1, allItems[0].Id);
        Assert.Equal("Помещение", allItems[0].Name);
        Assert.Equal("помещ.", allItems[0].ShortName);
        Assert.Equal("Помещение", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }
}