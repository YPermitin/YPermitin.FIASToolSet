using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataCollections;

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

    [Test]
    public void ReadItems()
    {
        var collection = new ApartmentTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(13, allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Id);
        Assert.AreEqual("Помещение", allItems[0].Name);
        Assert.AreEqual("помещ.", allItems[0].ShortName);
        Assert.AreEqual("Помещение", allItems[0].Description);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
}