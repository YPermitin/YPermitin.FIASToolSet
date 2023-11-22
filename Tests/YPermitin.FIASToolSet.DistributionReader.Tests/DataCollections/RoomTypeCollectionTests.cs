using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
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

    [Test]
    public void ReadItems()
    {
        var collection = new RoomTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(3, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не определено", allItems[0].Name);
        Assert.AreEqual("Не определено", allItems[0].Description);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(2011, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
}