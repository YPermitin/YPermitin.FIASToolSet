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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
    
    [Test]
    public void GetOperationTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetOperationTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(27, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
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
    
    [Test]
    public void GetAddressObjectAdmHierarchyTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjectsAdmHierarchy(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(114785646, allItems[0].Id);
        Assert.AreEqual(97678198, allItems[0].ObjectId);
        Assert.AreEqual(97670661, allItems[0].ParentObjectId);
        Assert.AreEqual(154016314, allItems[0].ChangeId);
        Assert.AreEqual(72, allItems[0].RegionCode);
        Assert.AreEqual(11, allItems[0].AreaCode);
        Assert.AreEqual(0, allItems[0].CityCode);
        Assert.AreEqual(0, allItems[0].PlaceCode);
        Assert.AreEqual(111, allItems[0].PlanCode);
        Assert.AreEqual(119, allItems[0].StreetCode);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(0, allItems[0].NextAddressObjectId);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual("1305522.1327981.97670661.97678198", allItems[0].Path);
        Assert.AreEqual(new DateOnly(2020,7,28), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2020,7,28), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetApartmentsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetApartments(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(17791460, allItems[0].Id);
        Assert.AreEqual(30820037, allItems[0].ObjectId);
        Assert.AreEqual(new Guid("9f63388a-add4-418c-9e34-03ae6f5ccd6a"), allItems[0].ObjectGuid);
        Assert.AreEqual(47172662, allItems[0].ChangeId);
        Assert.AreEqual("66", allItems[0].Number);
        Assert.AreEqual(2, allItems[0].ApartmentTypeId);
        Assert.AreEqual(10, allItems[0].OperationTypeId);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(0, allItems[0].NextAddressObjectId);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual(true, allItems[0].IsActual);
        Assert.AreEqual(new DateOnly(2018,8,31), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2018,8,31), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Test]
    public void GetApartmentParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetApartmentParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1331655940, allItems[0].Id);
        Assert.AreEqual(157009173, allItems[0].ObjectId);
        Assert.AreEqual(461999866, allItems[0].ChangeId);
        Assert.AreEqual(0, allItems[0].ChangeIdEnd);
        Assert.AreEqual(15, allItems[0].TypeId);
        Assert.AreEqual("98", allItems[0].Value);
        Assert.AreEqual(new DateOnly(2022,11,11), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2022,11,11), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetCarPlacesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetCarPlaces(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(35992, allItems[0].Id);
        Assert.AreEqual(98844842, allItems[0].ObjectId);
        Assert.AreEqual(new Guid("22932f52-4fab-4a3d-a368-5bf677ae07e8"), allItems[0].ObjectGuid);
        Assert.AreEqual(158065705, allItems[0].ChangeId);
        Assert.AreEqual("18", allItems[0].Number);
        Assert.AreEqual(10, allItems[0].OperationTypeId);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(0, allItems[0].NextAddressObjectId);
        Assert.AreEqual(false, allItems[0].IsActive);
        Assert.AreEqual(false, allItems[0].IsActual);
        Assert.AreEqual(new DateOnly(2021,3,16), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2020,10,27), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2021,3,16), allItems[0].EndDate);
    }
    
    [Test]
    public void GetCarPlaceParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetCarPlaceParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1331655, allItems[0].Id);
        Assert.AreEqual(157009, allItems[0].ObjectId);
        Assert.AreEqual(461999, allItems[0].ChangeId);
        Assert.AreEqual(0, allItems[0].ChangeIdEnd);
        Assert.AreEqual(15, allItems[0].TypeId);
        Assert.AreEqual("98", allItems[0].Value);
        Assert.AreEqual(new DateOnly(2022,11,11), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2022,11,11), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetChangeHistoryTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetChangeHistory(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(59620942, allItems[0].ObjectId);
        Assert.AreEqual(89049208, allItems[0].ChangeId);
        Assert.AreEqual(new Guid("e2bcf6e6-b14f-4ba6-b169-08e07efd274e"), allItems[0].AddressObjectGuid);
        Assert.AreEqual(20, allItems[0].OperationTypeId);
        Assert.AreEqual(new DateOnly(2016,7,14), allItems[0].ChangeDate);
    }
    
    [Test]
    public void GetHousesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetHouses(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(50188048, allItems[0].Id);
        Assert.AreEqual(82947269, allItems[0].ObjectId);
        Assert.AreEqual(new Guid("5d000166-0bfa-469a-907b-c6a5d22b324a"), allItems[0].ObjectGuid);
        Assert.AreEqual(123210629, allItems[0].ChangeId);
        Assert.AreEqual("19", allItems[0].HouseNumber);
        Assert.AreEqual(string.Empty, allItems[0].AddedHouseNumber1);
        Assert.AreEqual(string.Empty, allItems[0].AddedHouseNumber2);
        Assert.AreEqual(2, allItems[0].HouseTypeId);
        Assert.AreEqual(0, allItems[0].AddedHouseTypeId1);
        Assert.AreEqual(0, allItems[0].AddedHouseTypeId2);
        Assert.AreEqual(10, allItems[0].OperationTypeId);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(50188316, allItems[0].NextAddressObjectId);
        Assert.AreEqual(false, allItems[0].IsActive);
        Assert.AreEqual(false, allItems[0].IsActual);
        Assert.AreEqual(new DateOnly(2019, 7,10), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2013,10,21), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2014,1,4), allItems[0].EndDate);
    }
    
    [Test]
    public void GetHouseParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetHouseParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1324462328, allItems[0].Id);
        Assert.AreEqual(25489566, allItems[0].ObjectId);
        Assert.AreEqual(457704858, allItems[0].ChangeId);
        Assert.AreEqual(0, allItems[0].ChangeIdEnd);
        Assert.AreEqual(5, allItems[0].TypeId);
        Assert.AreEqual("627540", allItems[0].Value);
        Assert.AreEqual(new DateOnly(2022,10,22), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2022,10,22), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetMunHierarchyTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetMunHierarchy(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(111965448, allItems[0].Id);
        Assert.AreEqual(100263587, allItems[0].ObjectId);
        Assert.AreEqual(100248451, allItems[0].ParentObjectId);
        Assert.AreEqual(177353145, allItems[0].ChangeId);
        Assert.AreEqual("71701000001", allItems[0].OKTMO);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(0, allItems[0].NextAddressObjectId);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual("1305522.95250228.1308003.1310044.100248451.100263587", allItems[0].Path);
        Assert.AreEqual(new DateOnly(2021,2,11), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2021,2,11), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetNormativeDocumentsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetNormativeDocuments(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(22823132, allItems[0].Id);
        Assert.AreEqual("О присвоении адреса зданию" , allItems[0].Name);
        Assert.AreEqual(new DateOnly(2023,8,2), allItems[0].Date);
        Assert.AreEqual("3780-АР" , allItems[0].Number);
        Assert.AreEqual(24, allItems[0].TypeId);
        Assert.AreEqual(2, allItems[0].KindId);
        Assert.AreEqual(new DateOnly(2023,8,22), allItems[0].UpdateDate);
        Assert.AreEqual("Департамент земельных отношений и градостроительства Администрации города Тюмени" , allItems[0].OrgName);
        Assert.AreEqual(new DateOnly(2023,8,2), allItems[0].AccDate);
    }
    
    [Test]
    public void GetObjectsRegistryTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetObjectsRegistry(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(102175972, allItems[0].ObjectId);
        Assert.AreEqual(new Guid("d2106edd-0a71-4608-8cf7-3b56964bb297"), allItems[0].ObjectGuid);
        Assert.AreEqual(525409123, allItems[0].ChangeId);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual(10, allItems[0].LevelId);
        Assert.AreEqual(new DateOnly(2021,8,18), allItems[0].CreateDate);
        Assert.AreEqual(new DateOnly(2023,9,10), allItems[0].UpdateDate);
    }
    
    [Test]
    public void GetRoomsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetRooms(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(8313, allItems[0].Id);
        Assert.AreEqual(3308608, allItems[0].ObjectId);
        Assert.AreEqual(new Guid("0ba2d5fc-53d2-41cb-9b2d-654d200d837d"), allItems[0].ObjectGuid);
        Assert.AreEqual(6754677, allItems[0].ChangeId);
        Assert.AreEqual("2", allItems[0].RoomNumber);
        Assert.AreEqual(0, allItems[0].RoomTypeId);
        Assert.AreEqual(10, allItems[0].OperationTypeId);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(0, allItems[0].NextAddressObjectId);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual(true, allItems[0].IsActual);
        Assert.AreEqual(new DateOnly(2019, 4,12), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2015,11,12), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetRoomParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetRoomParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(640704957, allItems[0].Id);
        Assert.AreEqual(42588029, allItems[0].ObjectId);
        Assert.AreEqual(64323565, allItems[0].ChangeId);
        Assert.AreEqual(0, allItems[0].ChangeIdEnd);
        Assert.AreEqual(13, allItems[0].TypeId);
        Assert.AreEqual("717100000010067000040065000000000", allItems[0].Value);
        Assert.AreEqual(new DateOnly(2019,4,12), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2015,10,30), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetSteadsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetSteads(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(15514441, allItems[0].Id);
        Assert.AreEqual(104616649, allItems[0].ObjectId);
        Assert.AreEqual(new Guid("3ddaf49b-1189-45cc-a457-4d0326836e0a"), allItems[0].ObjectGuid);
        Assert.AreEqual(283567991, allItems[0].ChangeId);
        Assert.AreEqual("5/1", allItems[0].Number);
        Assert.AreEqual(10, allItems[0].OperationTypeId);
        Assert.AreEqual(0, allItems[0].PreviousAddressObjectId);
        Assert.AreEqual(0, allItems[0].NextAddressObjectId);
        Assert.AreEqual(true, allItems[0].IsActive);
        Assert.AreEqual(true, allItems[0].IsActual);
        Assert.AreEqual(new DateOnly(2022, 6,1), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2022,6,1), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    [Test]
    public void GetSteadParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetSteadParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.IsNotEmpty(allItems);
        Assert.AreEqual(1, allItems.Count);
        Assert.AreEqual(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.AreEqual(1442664443, allItems[0].Id);
        Assert.AreEqual(160471707, allItems[0].ObjectId);
        Assert.AreEqual(547011578, allItems[0].ChangeId);
        Assert.AreEqual(0, allItems[0].ChangeIdEnd);
        Assert.AreEqual(8, allItems[0].TypeId);
        Assert.AreEqual("72:17:1707006:11532", allItems[0].Value);
        Assert.AreEqual(new DateOnly(2023,9,26), allItems[0].UpdateDate);
        Assert.AreEqual(new DateOnly(2023,9,26), allItems[0].StartDate);
        Assert.AreEqual(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    #endregion
}