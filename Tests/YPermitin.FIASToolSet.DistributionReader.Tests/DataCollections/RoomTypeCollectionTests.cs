using System;
using System.IO;
using System.Linq;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class RoomTypeCollectionTests
{
    private readonly string _dataFile;

    public RoomTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_ROOM_TYPES_20230706_190ba38d-fe2d-4afc-a5f7-6330669f570e.XML"
        );
    }

    [Fact]
    public void ReadItems()
    {
        var collection = new RoomTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(3, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не определено", allItems[0].Name);
        Assert.Equal("Не определено", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.Equal(new DateOnly(2011, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }
}