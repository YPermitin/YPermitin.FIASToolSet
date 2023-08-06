using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataCollections;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class OperationTypeCollectionTests
{
    private readonly string _dataFile;

    public OperationTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_OPERATION_TYPES_20230706_39508983-2c70-485d-98ac-53bad8c38ec5.XML"
        );
    }

    [Test]
    public void ReadItems()
    {
        var collection = new OperationTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(27, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не определено", allItems[0].Name);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
}