using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataCollections;
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

    [Test]
    public void ReadItems()
    {
        var collection = new AddressObjectTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(421, allItems.Count);
        
        Assert.AreEqual(5, allItems[0].Id);
        Assert.AreEqual(1, allItems[0].Level);
        Assert.AreEqual("Автономная область", allItems[0].Name);
        Assert.AreEqual("Аобл", allItems[0].ShortName);
        Assert.AreEqual("Автономная область", allItems[0].Description);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
    }
}