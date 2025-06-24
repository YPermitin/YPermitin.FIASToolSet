using System;
using System.IO;
using System.Linq;

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

    [Fact]
    public void GetVersionAsStringTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        string version = reader.GetVersionAsString();

        Assert.NotNull(version);
    }

    [Fact]
    public void GetVersionTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        int version = reader.GetVersion();

        Assert.True(version > 0);
    }

    [Fact]
    public void GetRegions()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var availableRegions = reader.GetRegions();

        Assert.NotNull(availableRegions);
        Assert.True(availableRegions.Count > 0);
    }

    #region BaseCatalogs

    [Fact]
    public void GetNormativeDocKindsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetNormativeDocKinds();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(4, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не определено", allItems[0].Name);
    }

    [Fact]
    public void GetNormativeDocTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetNormativeDocTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(25, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не указан", allItems[0].Name);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2016, 3, 31), allItems[0].EndDate);
    }

    [Fact]
    public void GetObjectLevelsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetObjectLevels();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(17, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1, allItems[0].Level);
        Assert.Equal("Субъект РФ", allItems[0].Name);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }

    [Fact]
    public void GetRoomTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetRoomTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(3, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(0, allItems[0].Id);
        Assert.Equal("Не определено", allItems[0].Name);
        Assert.Equal("Не определено", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.Equal(new DateOnly(2011, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }

    [Fact]
    public void GetApartmentTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetApartmentTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(13, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1, allItems[0].Id);
        Assert.Equal("Помещение", allItems[0].Name);
        Assert.Equal("помещ.", allItems[0].ShortName);
        Assert.Equal("Помещение", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
    }

    [Fact]
    public void GetHouseTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetHouseTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(14, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1, allItems[0].Id);
        Assert.Equal("Владение", allItems[0].Name);
        Assert.Equal("влд.", allItems[0].ShortName);
        Assert.Equal("Владение", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
        Assert.Equal(false, allItems[0].IsActive);
    }

    [Fact]
    public void GetOperationTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetOperationTypes();
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

    [Fact]
    public void GetAddressObjectTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetAddressObjectTypes();
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(421, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(5, allItems[0].Id);
        Assert.Equal(1, allItems[0].Level);
        Assert.Equal("Автономная область", allItems[0].Name);
        Assert.Equal("Аобл", allItems[0].ShortName);
        Assert.Equal("Автономная область", allItems[0].Description);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2015, 11, 5), allItems[0].EndDate);
        Assert.Equal(new DateOnly(1900, 1, 1), allItems[0].UpdateDate);
    }

    [Fact]
    public void GetParameterTypesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var collection = reader.GetParameterTypes();
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

    #endregion

    #region ClassifierData

    [Fact]
    public void GetAddressObjectsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjects(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(7, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1937556, allItems[0].Id);
        Assert.Equal("Белая", allItems[0].Name);
        Assert.Equal("ул", allItems[0].TypeName);
        Assert.Equal(new DateOnly(2022, 1, 13), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079, 6, 6), allItems[0].EndDate);
        Assert.Equal(new DateOnly(2022, 1, 13), allItems[0].UpdateDate);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal(true, allItems[0].IsActual);
    }

    [Fact]
    public void GetAddressObjectDivisionsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjectDivisions(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(180, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(8565, allItems[0].Id);
        Assert.Equal(1308258, allItems[0].ParentId);
        Assert.Equal(1308331, allItems[0].ChildId);
        Assert.Equal(3581246, allItems[0].ChangeId);
    }

    [Fact]
    public void GetAddressObjectParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjectParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(20263101, allItems[0].Id);
        Assert.Equal(1310372, allItems[0].ObjectId);
        Assert.Equal(3587942, allItems[0].ChangeId);
        Assert.Equal(0, allItems[0].ChangeIdEnd);
        Assert.Equal(6, allItems[0].TypeId);
        Assert.Equal("71410000000", allItems[0].Value);
        Assert.Equal(new DateOnly(2016,3,23), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(1900,1,1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetAddressObjectAdmHierarchyTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetAddressObjectsAdmHierarchy(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(114785646, allItems[0].Id);
        Assert.Equal(97678198, allItems[0].ObjectId);
        Assert.Equal(97670661, allItems[0].ParentObjectId);
        Assert.Equal(154016314, allItems[0].ChangeId);
        Assert.Equal(72, allItems[0].RegionCode);
        Assert.Equal(11, allItems[0].AreaCode);
        Assert.Equal(0, allItems[0].CityCode);
        Assert.Equal(0, allItems[0].PlaceCode);
        Assert.Equal(111, allItems[0].PlanCode);
        Assert.Equal(119, allItems[0].StreetCode);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(0, allItems[0].NextAddressObjectId);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal("1305522.1327981.97670661.97678198", allItems[0].Path);
        Assert.Equal(new DateOnly(2020,7,28), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2020,7,28), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetApartmentsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetApartments(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(17791460, allItems[0].Id);
        Assert.Equal(30820037, allItems[0].ObjectId);
        Assert.Equal(new Guid("9f63388a-add4-418c-9e34-03ae6f5ccd6a"), allItems[0].ObjectGuid);
        Assert.Equal(47172662, allItems[0].ChangeId);
        Assert.Equal("66", allItems[0].Number);
        Assert.Equal(2, allItems[0].ApartmentTypeId);
        Assert.Equal(10, allItems[0].OperationTypeId);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(0, allItems[0].NextAddressObjectId);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal(true, allItems[0].IsActual);
        Assert.Equal(new DateOnly(2018,8,31), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2018,8,31), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetApartmentParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetApartmentParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1331655940, allItems[0].Id);
        Assert.Equal(157009173, allItems[0].ObjectId);
        Assert.Equal(461999866, allItems[0].ChangeId);
        Assert.Equal(0, allItems[0].ChangeIdEnd);
        Assert.Equal(15, allItems[0].TypeId);
        Assert.Equal("98", allItems[0].Value);
        Assert.Equal(new DateOnly(2022,11,11), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2022,11,11), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetCarPlacesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetCarPlaces(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(35992, allItems[0].Id);
        Assert.Equal(98844842, allItems[0].ObjectId);
        Assert.Equal(new Guid("22932f52-4fab-4a3d-a368-5bf677ae07e8"), allItems[0].ObjectGuid);
        Assert.Equal(158065705, allItems[0].ChangeId);
        Assert.Equal("18", allItems[0].Number);
        Assert.Equal(10, allItems[0].OperationTypeId);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(0, allItems[0].NextAddressObjectId);
        Assert.Equal(false, allItems[0].IsActive);
        Assert.Equal(false, allItems[0].IsActual);
        Assert.Equal(new DateOnly(2021,3,16), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2020,10,27), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2021,3,16), allItems[0].EndDate);
    }

    [Fact]
    public void GetCarPlaceParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetCarPlaceParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1331655, allItems[0].Id);
        Assert.Equal(157009, allItems[0].ObjectId);
        Assert.Equal(461999, allItems[0].ChangeId);
        Assert.Equal(0, allItems[0].ChangeIdEnd);
        Assert.Equal(15, allItems[0].TypeId);
        Assert.Equal("98", allItems[0].Value);
        Assert.Equal(new DateOnly(2022,11,11), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2022,11,11), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetChangeHistoryTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetChangeHistory(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(59620942, allItems[0].ObjectId);
        Assert.Equal(89049208, allItems[0].ChangeId);
        Assert.Equal(new Guid("e2bcf6e6-b14f-4ba6-b169-08e07efd274e"), allItems[0].AddressObjectGuid);
        Assert.Equal(20, allItems[0].OperationTypeId);
        Assert.Equal(new DateOnly(2016,7,14), allItems[0].ChangeDate);
        Assert.Equal(new byte[] { 236, 149, 24, 136, 187, 127, 44, 39, 19, 82, 37, 97, 22, 48, 109, 230 }, 
            allItems[0].HashMD5);
    }

    [Fact]
    public void GetHousesTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetHouses(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(50188048, allItems[0].Id);
        Assert.Equal(82947269, allItems[0].ObjectId);
        Assert.Equal(new Guid("5d000166-0bfa-469a-907b-c6a5d22b324a"), allItems[0].ObjectGuid);
        Assert.Equal(123210629, allItems[0].ChangeId);
        Assert.Equal("19", allItems[0].HouseNumber);
        Assert.Equal(string.Empty, allItems[0].AddedHouseNumber1);
        Assert.Equal(string.Empty, allItems[0].AddedHouseNumber2);
        Assert.Equal(2, allItems[0].HouseTypeId);
        Assert.Equal(0, allItems[0].AddedHouseTypeId1);
        Assert.Equal(0, allItems[0].AddedHouseTypeId2);
        Assert.Equal(10, allItems[0].OperationTypeId);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(50188316, allItems[0].NextAddressObjectId);
        Assert.Equal(false, allItems[0].IsActive);
        Assert.Equal(false, allItems[0].IsActual);
        Assert.Equal(new DateOnly(2019, 7,10), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2013,10,21), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2014,1,4), allItems[0].EndDate);
    }

    [Fact]
    public void GetHouseParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetHouseParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1324462328, allItems[0].Id);
        Assert.Equal(25489566, allItems[0].ObjectId);
        Assert.Equal(457704858, allItems[0].ChangeId);
        Assert.Equal(0, allItems[0].ChangeIdEnd);
        Assert.Equal(5, allItems[0].TypeId);
        Assert.Equal("627540", allItems[0].Value);
        Assert.Equal(new DateOnly(2022,10,22), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2022,10,22), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetMunHierarchyTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetMunHierarchy(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(111965448, allItems[0].Id);
        Assert.Equal(100263587, allItems[0].ObjectId);
        Assert.Equal(100248451, allItems[0].ParentObjectId);
        Assert.Equal(177353145, allItems[0].ChangeId);
        Assert.Equal("71701000001", allItems[0].OKTMO);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(0, allItems[0].NextAddressObjectId);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal("1305522.95250228.1308003.1310044.100248451.100263587", allItems[0].Path);
        Assert.Equal(new DateOnly(2021,2,11), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2021,2,11), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetNormativeDocumentsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetNormativeDocuments(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(22823132, allItems[0].Id);
        Assert.Equal("О присвоении адреса зданию" , allItems[0].Name);
        Assert.Equal(new DateOnly(2023,8,2), allItems[0].Date);
        Assert.Equal("3780-АР" , allItems[0].Number);
        Assert.Equal(24, allItems[0].TypeId);
        Assert.Equal(2, allItems[0].KindId);
        Assert.Equal(new DateOnly(2023,8,22), allItems[0].UpdateDate);
        Assert.Equal("Департамент земельных отношений и градостроительства Администрации города Тюмени" , allItems[0].OrgName);
        Assert.Equal(new DateOnly(2023,8,2), allItems[0].AccDate);
    }

    [Fact]
    public void GetObjectsRegistryTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetObjectsRegistry(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(102175972, allItems[0].ObjectId);
        Assert.Equal(new Guid("d2106edd-0a71-4608-8cf7-3b56964bb297"), allItems[0].ObjectGuid);
        Assert.Equal(525409123, allItems[0].ChangeId);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal(10, allItems[0].LevelId);
        Assert.Equal(new DateOnly(2021,8,18), allItems[0].CreateDate);
        Assert.Equal(new DateOnly(2023,9,10), allItems[0].UpdateDate);
        Assert.Equal(new byte[] { 48, 31, 144, 57, 136, 35, 186, 199, 17, 111, 113, 146, 108, 21, 21, 206 }, 
            allItems[0].HashMD5);
    }

    [Fact]
    public void GetRoomsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetRooms(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(8313, allItems[0].Id);
        Assert.Equal(3308608, allItems[0].ObjectId);
        Assert.Equal(new Guid("0ba2d5fc-53d2-41cb-9b2d-654d200d837d"), allItems[0].ObjectGuid);
        Assert.Equal(6754677, allItems[0].ChangeId);
        Assert.Equal("2", allItems[0].RoomNumber);
        Assert.Equal(0, allItems[0].RoomTypeId);
        Assert.Equal(10, allItems[0].OperationTypeId);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(0, allItems[0].NextAddressObjectId);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal(true, allItems[0].IsActual);
        Assert.Equal(new DateOnly(2019, 4,12), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2015,11,12), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetRoomParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetRoomParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(640704957, allItems[0].Id);
        Assert.Equal(42588029, allItems[0].ObjectId);
        Assert.Equal(64323565, allItems[0].ChangeId);
        Assert.Equal(0, allItems[0].ChangeIdEnd);
        Assert.Equal(13, allItems[0].TypeId);
        Assert.Equal("717100000010067000040065000000000", allItems[0].Value);
        Assert.Equal(new DateOnly(2019,4,12), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2015,10,30), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetSteadsTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetSteads(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(15514441, allItems[0].Id);
        Assert.Equal(104616649, allItems[0].ObjectId);
        Assert.Equal(new Guid("3ddaf49b-1189-45cc-a457-4d0326836e0a"), allItems[0].ObjectGuid);
        Assert.Equal(283567991, allItems[0].ChangeId);
        Assert.Equal("5/1", allItems[0].Number);
        Assert.Equal(10, allItems[0].OperationTypeId);
        Assert.Equal(0, allItems[0].PreviousAddressObjectId);
        Assert.Equal(0, allItems[0].NextAddressObjectId);
        Assert.Equal(true, allItems[0].IsActive);
        Assert.Equal(true, allItems[0].IsActual);
        Assert.Equal(new DateOnly(2022, 6,1), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2022,6,1), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }

    [Fact]
    public void GetSteadParametersTest()
    {
        FIASDistributionReader reader = new FIASDistributionReader(_workingDirectory);

        var regions = reader.GetRegions();
        var testRegion = regions.First();
        var collection = reader.GetSteadParameters(testRegion);
        var allItems = collection.ToList();

        Assert.NotNull(allItems);
        Assert.NotEmpty(allItems);
        Assert.Equal(1, allItems.Count);
        Assert.Equal(collection.CalculateCollectionSize(), allItems.Count);
        
        Assert.Equal(1442664443, allItems[0].Id);
        Assert.Equal(160471707, allItems[0].ObjectId);
        Assert.Equal(547011578, allItems[0].ChangeId);
        Assert.Equal(0, allItems[0].ChangeIdEnd);
        Assert.Equal(8, allItems[0].TypeId);
        Assert.Equal("72:17:1707006:11532", allItems[0].Value);
        Assert.Equal(new DateOnly(2023,9,26), allItems[0].UpdateDate);
        Assert.Equal(new DateOnly(2023,9,26), allItems[0].StartDate);
        Assert.Equal(new DateOnly(2079,6,6), allItems[0].EndDate);
    }
    
    #endregion
}