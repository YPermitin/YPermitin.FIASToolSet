using System;
using System.IO;
using System.Linq;
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

    [Fact]
    public void ReadItems()
    {
        var collection = new ParameterTypeCollection(_dataFile);
        var allItems = collection.ToList();
        
        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(19, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1, allItems[0].Id);
        Assert.Equal("ИФНС ФЛ", allItems[0].Name);
        Assert.Equal("ИФНС ФЛ", allItems[0].Description);
        Assert.Equal("IFNSFL", allItems[0].Code);
        Assert.Equal(new DateOnly(2011, 11, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.Equal(new DateOnly(2018, 6, 15), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }
}