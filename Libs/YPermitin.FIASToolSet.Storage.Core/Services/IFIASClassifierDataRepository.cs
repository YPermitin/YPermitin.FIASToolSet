using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;

namespace YPermitin.FIASToolSet.Storage.Core.Services;

public interface IFIASClassifierDataRepository
{
    #region AddressObjects

    Task<List<AddressObject>> GetAddressObjects(List<int> ids = null);
    Task<AddressObject> GetAddressObject(int id);
    Task<bool> AddressObjectExists(int id);
    void AddAddressObject(AddressObject objectLevel);
    void UpdateAddressObject(AddressObject objectLevel);
    void RemoveAddressObject(AddressObject objectLevel);

    #endregion
    
    #region AddressObjectDivision

    Task<List<AddressObjectDivision>> GetAddressObjectDivisions(List<int> ids = null);
    Task<AddressObjectDivision> GetAddressObjectDivision(int id);
    Task<bool> AddressObjectDivisionExists(int id);
    void AddAddressObjectDivision(AddressObjectDivision objectLevel);
    void UpdateAddressObjectDivision(AddressObjectDivision objectLevel);
    void RemoveAddressObjectDivision(AddressObjectDivision objectLevel);

    #endregion
    
    #region AddressObjectParameter

    Task<List<AddressObjectParameter>> GetAddressObjectParameters(List<int> ids = null);
    Task<AddressObjectParameter> GetAddressObjectParameter(int id);
    Task<bool> AddressObjectParameterExists(int id);
    void AddAddressObjectParameter(AddressObjectParameter addressObjectParameter);
    void UpdateAddressObjectParameter(AddressObjectParameter addressObjectParameter);
    void RemoveAddressObjectParameter(AddressObjectParameter addressObjectParameter);

    #endregion
    
    #region AddressObjectAdmHierarchy

    Task<List<AddressObjectAdmHierarchy>> GetAddressObjectsAdmHierarchy(List<int> ids = null);

    Task<AddressObjectAdmHierarchy> GetAddressObjectAdmHierarchy(int id);

    Task<bool> AddressObjectAdmHierarchyExists(int id);

    void AddAddressObjectAdmHierarchy(AddressObjectAdmHierarchy addressObjectAdmHierarchy);

    void UpdateAddressObjectAdmHierarchy(AddressObjectAdmHierarchy addressObjectAdmHierarchy);

    void RemoveAddressObjectAdmHierarchy(AddressObjectAdmHierarchy addressObjectAdmHierarchy);

    #endregion
    
    #region Apartment

    Task<List<Apartment>> GetApartments(List<int> ids = null);

    Task<Apartment> GetApartment(int id);

    Task<bool> ApartmentExists(int id);

    void AddApartment(Apartment apartment);

    void UpdateApartment(Apartment apartment);

    void RemoveApartment(Apartment apartment);

    #endregion
    
    #region ApartmentParameter

    Task<List<ApartmentParameter>> GetApartmentParameters(List<int> ids = null);

    Task<ApartmentParameter> GetApartmentParameter(int id);

    Task<bool> ApartmentParameterExists(int id);

    void AddApartmentParameter(ApartmentParameter apartmentParameter);

    void UpdateApartmentParameter(ApartmentParameter apartmentParameter);

    void RemoveApartmentParameter(ApartmentParameter apartmentParameter);

    #endregion
    
    #region CarPlace

    Task<List<CarPlace>> GetCarPlaces(List<int> ids = null);

    Task<CarPlace> GetCarPlace(int id);

    Task<bool> CarPlaceExists(int id);

    void AddCarPlace(CarPlace carPlace);

    void UpdateCarPlace(CarPlace carPlace);

    void RemoveCarPlace(CarPlace carPlace);

    #endregion
    
    #region CarPlaceParameter

    Task<List<CarPlaceParameter>> GetCarPlaceParameters(List<int> ids = null);

    Task<CarPlaceParameter> GetCarPlaceParameter(int id);

    Task<bool> CarPlaceParameterExists(int id);

    void AddCarPlaceParameter(CarPlaceParameter carPlaceParameter);

    void UpdateCarPlaceParameter(CarPlaceParameter carPlaceParameter);

    void RemoveCarPlaceParameter(CarPlaceParameter carPlaceParameter);

    #endregion
    
    #region House

    Task<List<House>> GetHouses(List<int> ids = null);

    Task<House> GetHouse(int id);

    Task<bool> HouseExists(int id);

    void AddHouse(House house);

    void UpdateHouse(House house);

    void RemoveHouse(House house);

    #endregion
    
    #region HouseParameter

    Task<List<HouseParameter>> GetHouseParameters(List<int> ids = null);

    Task<HouseParameter> GetHouseParameter(int id);

    Task<bool> HouseParameterExists(int id);

    void AddHouseParameter(HouseParameter houseParameter);

    void UpdateHouseParameter(HouseParameter houseParameter);

    void RemoveHouseParameter(HouseParameter houseParameter);

    #endregion
    
    #region Room

    Task<List<Room>> GetRooms(List<int> ids = null);

    Task<Room> GetRoom(int id);

    Task<bool> RoomExists(int id);

    void AddRoom(Room room);

    void UpdateRoom(Room room);

    void RemoveRoom(Room room);

    #endregion
    
    #region RoomParameter

    Task<List<RoomParameter>> GetRoomParameters(List<int> ids = null);

    Task<RoomParameter> GetRoomParameter(int id);

    Task<bool> RoomParameterExists(int id);

    void AddRoomParameter(RoomParameter roomParameter);

    void UpdateRoomParameter(RoomParameter roomParameter);

    void RemoveRoomParameter(RoomParameter roomParameter);

    #endregion
    
    #region Stead

    Task<List<Stead>> GetSteads(List<int> ids = null);

    Task<Stead> GetStead(int id);

    Task<bool> SteadExists(int id);

    void AddStead(Stead stead);

    void UpdateStead(Stead stead);

    void RemoveStead(Stead stead);

    #endregion
    
    #region SteadParameter

    Task<List<SteadParameter>> GetSteadParameters(List<int> ids = null);

    Task<SteadParameter> GetSteadParameter(int id);

    Task<bool> SteadParameterExists(int id);

    void AddSteadParameter(SteadParameter steadParameter);

    void UpdateSteadParameter(SteadParameter steadParameter);

    void RemoveSteadParameter(SteadParameter steadParameter);

    #endregion
    
    Task<bool> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> SaveAsync();
    Task SaveWithIdentityInsertAsync<T>();
    void ClearChangeTracking();
}