using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataCollections;
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

    [Test]
    public void ReadItems()
    {
        var collection = new NormativeDocTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(25, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не указан", allItems[0].Name);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2016, 3, 31), allItems[0].EndDate);
    }
}