using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
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

    [Test]
    public void ReadItems()
    {
        var collection = new HouseTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(14, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Id);
        Assert.AreEqual("Владение", allItems[0].Name);
        Assert.AreEqual("влд.", allItems[0].ShortName);
        Assert.AreEqual("Владение", allItems[0].Description);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(false, allItems[0].IsActive);
    }
}