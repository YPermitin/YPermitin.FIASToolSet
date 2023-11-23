using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using YPermitin.FIASToolSet.DistributionReader.DataCollections.BaseCatalogs;

namespace YPermitin.FIASToolSet.DistributionReader.Tests.DataCollections;

public class ParameterTypeCollectionTests
{
    private readonly string _dataFile;

    public ParameterTypeCollectionTests()
    {
        _dataFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData",
            "AS_PARAM_TYPES_20230706_5c240782-bd40-44dd-a3fd-2c84c42c6c22.XML"
        );
    }

    [Test]
    public void ReadItems()
    {
        var collection = new ParameterTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(19, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Id);
        Assert.AreEqual("ИФНС ФЛ", allItems[0].Name);
        Assert.AreEqual("ИФНС ФЛ", allItems[0].Description);
        Assert.AreEqual("IFNSFL", allItems[0].Code);
        Assert.AreEqual(new DateOnly(2011, 11, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(2018, 6, 15), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
}