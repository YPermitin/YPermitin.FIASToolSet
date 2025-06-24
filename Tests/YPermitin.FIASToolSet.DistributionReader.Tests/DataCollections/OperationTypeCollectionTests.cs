using System;
using System.IO;
using System.Linq;
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

    [Fact]
    public void ReadItems()
    {
        var collection = new OperationTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(27, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не определено", allItems[0].Name);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }
}