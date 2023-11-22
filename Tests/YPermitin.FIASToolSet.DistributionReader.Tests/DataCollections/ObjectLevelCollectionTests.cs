using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class ObjectLevelCollectionTests
{
    private readonly string _dataFile;

    public ObjectLevelCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_OBJECT_LEVELS_20230706_4d4c4dfd-b0ce-4596-b923-e687d5118e42.XML"
        );
    }

    [Test]
    public void ReadItems()
    {
        var collection = new ObjectLevelCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(17, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Level);
        Assert.AreEqual("Субъект РФ", allItems[0].Name);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
}