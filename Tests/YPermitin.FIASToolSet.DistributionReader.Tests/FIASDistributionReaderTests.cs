using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace YPermitin.FIASToolSet.DistributionReader.Tests;

public class Tests
{
    private readonly string _workingDirectory;

    public Tests()
    {
        _workingDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "SampleData"
        );
    }
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetVersionAsStringTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        string version = reader.GetVersionAsString();

        Assert.IsNotNull(version);
    }
    
    [Test]
    public void GetVersionTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        int version = reader.GetVersion();

        Assert.True(version > 0);
    }
    
    [Test]
    public void GetRegions()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var availableRegions = reader.GetRegions();

        Assert.NotNull(availableRegions);
        Assert.True(availableRegions.Count > 0);
    }

    #region BaseCatalogs
    
    [Test]
    public void GetNormativeDocKindsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetNormativeDocKinds();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(4, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не определено", allItems[0].Name);
    }
    
    [Test]
    public void GetNormativeDocTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetNormativeDocTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(25, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не указан", allItems[0].Name);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2016, 3, 31), allItems[0].EndDate);
    }
    
    [Test]
    public void GetObjectLevelsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetObjectLevels();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(17, allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Level);
        Assert.AreEqual("Субъект РФ", allItems[0].Name);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
    
    [Test]
    public void GetRoomTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetRoomTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(3, allItems.Count);
        
        Assert.AreEqual(0, allItems[0].Id);
        Assert.AreEqual("Не определено", allItems[0].Name);
        Assert.AreEqual("Не определено", allItems[0].Description);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(2011, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
    
    [Test]
    public void GetApartmentTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetApartmentTypes();
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
    
    [Test]
    public void GetHouseTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetHouseTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(14, allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Id);
        Assert.AreEqual("Владение", allItems[0].Name);
        Assert.AreEqual("влд.", allItems[0].ShortName);
        Assert.AreEqual("Владение", allItems[0].Description);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.AreEqual(false, allItems[0].IsActive);
    }
    
    [Test]
    public void GetOperationTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetOperationTypes();
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
    
    [Test]
    public void GetAddressObjectTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetAddressObjectTypes();
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
    
    [Test]
    public void GetParameterTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetParameterTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(19, allItems.Count);
        
        Assert.AreEqual(1, allItems[0].Id);
        Assert.AreEqual("ИФНС ФЛ", allItems[0].Name);
        Assert.AreEqual("ИФНС ФЛ", allItems[0].Description);
        Assert.AreEqual("IFNSFL", allItems[0].Code);
        Assert.AreEqual(new DateOnly(2011, 11, 1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(2018, 6, 15), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
    }
    
    #endregion

    #region ClassifierData

    [Test]
    public void GetAddressObjectsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjects(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(7, allItems.Count);
        
        Assert.AreEqual(1937556, allItems[0].Id);
        Assert.AreEqual("Белая", allItems[0].Name);
        Assert.AreEqual("ул", allItems[0].TypeName);
        Assert.AreEqual(new DateOnly(2022, 1, 13), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.AreEqual(new DateOnly(2022, 1, 13), allItems[0].UpdateDate);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual(true, allItems[0].IsActual);
    }
    
    [Test]
    public void GetAddressObjectDivisionsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjectDivisions(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(180, allItems.Count);
        
        Assert.AreEqual(8565, allItems[0].Id);
        Assert.AreEqual(1308258, allItems[0].ParentId);
        Assert.AreEqual(1308331, allItems[0].ChildId);
        Assert.AreEqual(3581246, allItems[0].ChangeId);
    }
    
    [Test]
    public void GetAddressObjectParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjectParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        
        Assert.AreEqual(20263101, allItems[0].Id);
        Assert.AreEqual(1310372, allItems[0].ObjectId);
        Assert.AreEqual(3587942, allItems[0].ChangeId);
        Assert.AreEqual(0, allItems[0].ChangeIdEnd);
        Assert.AreEqual(6, allItems[0].TypeId);
        Assert.AreEqual("71410000000", allItems[0].Value);
        Assert.AreEqual(new DateOnly(2016,3,23), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(1900,1,1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    #endregion
}