using Microsoft.EntityFrameworkCore;
using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.SQLServer.DbContexts;

namespace YPermitin.FIASToolSet.Storage.SQLServer.Services;

public class FIASClassifierDataRepository : CommonRepository, IFIASClassifierDataRepository
{
    public FIASClassifierDataRepository(FIASToolSetServiceContext context) : base(context)
    {
    }

    #region AddressObjects
    
    public async Task<List<AddressObject>> GetAddressObjects(List<int> ids = null)
    {
        var query = _context.FIASAddressObjects
            .AsQueryable()
            .AsNoTracking();

        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<AddressObject> GetAddressObject(int id)
    {
        var query = _context.FIASAddressObjects
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AddressObjectExists(int id)
    {
        var query = _context.FIASAddressObjects
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddAddressObject(AddressObject objectLevel)
    {
        _context.Entry(objectLevel).State = EntityState.Added;
    }

    public void UpdateAddressObject(AddressObject objectLevel)
    {
        _context.Entry(objectLevel).State = EntityState.Modified;
    }

    public void RemoveAddressObject(AddressObject objectLevel)
    {
        _context.Entry(objectLevel).State = EntityState.Deleted;
    }
    
    #endregion
    
    #region AddressObjectDivision

    public async Task<List<AddressObjectDivision>> GetAddressObjectDivisions(List<int> ids = null)
    {
        var query = _context.FIASAddressObjectDivisions
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<AddressObjectDivision> GetAddressObjectDivision(int id)
    {
        var query = _context.FIASAddressObjectDivisions
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AddressObjectDivisionExists(int id)
    {
        var query = _context.FIASAddressObjectDivisions
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddAddressObjectDivision(AddressObjectDivision addressObjectDivision)
    {
        _context.Entry(addressObjectDivision).State = EntityState.Added;
    }

    public void UpdateAddressObjectDivision(AddressObjectDivision addressObjectDivision)
    {
        _context.Entry(addressObjectDivision).State = EntityState.Modified;
    }

    public void RemoveAddressObjectDivision(AddressObjectDivision addressObjectDivision)
    {
        _context.Entry(addressObjectDivision).State = EntityState.Deleted;
    }

    #endregion
    
    #region AddressObjectParameter

    public async Task<List<AddressObjectParameter>> GetAddressObjectParameters(List<int> ids = null)
    {
        var query = _context.FIASAddressObjectParameters
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<AddressObjectParameter> GetAddressObjectParameter(int id)
    {
        var query = _context.FIASAddressObjectParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AddressObjectParameterExists(int id)
    {
        var query = _context.FIASAddressObjectParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddAddressObjectParameter(AddressObjectParameter addressObjectParameter)
    {
        _context.Entry(addressObjectParameter).State = EntityState.Added;
    }

    public void UpdateAddressObjectParameter(AddressObjectParameter addressObjectParameter)
    {
        _context.Entry(addressObjectParameter).State = EntityState.Modified;
    }

    public void RemoveAddressObjectParameter(AddressObjectParameter addressObjectParameter)
    {
        _context.Entry(addressObjectParameter).State = EntityState.Deleted;
    }

    #endregion
    
    #region AddressObjectAdmHierarchy

    public async Task<List<AddressObjectAdmHierarchy>> GetAddressObjectsAdmHierarchy(List<int> ids = null)
    {
        var query = _context.FIASAddressObjectsAdmHierarchy
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<AddressObjectAdmHierarchy> GetAddressObjectAdmHierarchy(int id)
    {
        var query = _context.FIASAddressObjectsAdmHierarchy
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AddressObjectAdmHierarchyExists(int id)
    {
        var query = _context.FIASAddressObjectsAdmHierarchy
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddAddressObjectAdmHierarchy(AddressObjectAdmHierarchy addressObjectAdmHierarchy)
    {
        _context.Entry(addressObjectAdmHierarchy).State = EntityState.Added;
    }

    public void UpdateAddressObjectAdmHierarchy(AddressObjectAdmHierarchy addressObjectAdmHierarchy)
    {
        _context.Entry(addressObjectAdmHierarchy).State = EntityState.Modified;
    }

    public void RemoveAddressObjectAdmHierarchy(AddressObjectAdmHierarchy addressObjectAdmHierarchy)
    {
        _context.Entry(addressObjectAdmHierarchy).State = EntityState.Deleted;
    }

    #endregion
    
    #region Apartment

    public async Task<List<Apartment>> GetApartments(List<int> ids = null)
    {
        var query = _context.FIASApartments
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<Apartment> GetApartment(int id)
    {
        var query = _context.FIASApartments
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> ApartmentExists(int id)
    {
        var query = _context.FIASApartments
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddApartment(Apartment apartment)
    {
        _context.Entry(apartment).State = EntityState.Added;
    }

    public void UpdateApartment(Apartment apartment)
    {
        _context.Entry(apartment).State = EntityState.Modified;
    }

    public void RemoveApartment(Apartment apartment)
    {
        _context.Entry(apartment).State = EntityState.Deleted;
    }

    #endregion
    
    #region ApartmentParameter

    public async Task<List<ApartmentParameter>> GetApartmentParameters(List<int> ids = null)
    {
        var query = _context.FIASApartmentParameters
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<ApartmentParameter> GetApartmentParameter(int id)
    {
        var query = _context.FIASApartmentParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> ApartmentParameterExists(int id)
    {
        var query = _context.FIASApartmentParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddApartmentParameter(ApartmentParameter apartmentParameter)
    {
        _context.Entry(apartmentParameter).State = EntityState.Added;
    }

    public void UpdateApartmentParameter(ApartmentParameter apartmentParameter)
    {
        _context.Entry(apartmentParameter).State = EntityState.Modified;
    }

    public void RemoveApartmentParameter(ApartmentParameter apartmentParameter)
    {
        _context.Entry(apartmentParameter).State = EntityState.Deleted;
    }

    #endregion
    
    #region CarPlace

    public async Task<List<CarPlace>> GetCarPlaces(List<int> ids = null)
    {
        var query = _context.FIASCarPlaces
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<CarPlace> GetCarPlace(int id)
    {
        var query = _context.FIASCarPlaces
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> CarPlaceExists(int id)
    {
        var query = _context.FIASCarPlaces
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddCarPlace(CarPlace carPlace)
    {
        _context.Entry(carPlace).State = EntityState.Added;
    }

    public void UpdateCarPlace(CarPlace carPlace)
    {
        _context.Entry(carPlace).State = EntityState.Modified;
    }

    public void RemoveCarPlace(CarPlace carPlace)
    {
        _context.Entry(carPlace).State = EntityState.Deleted;
    }

    #endregion
    
    #region CarPlaceParameter

    public async Task<List<CarPlaceParameter>> GetCarPlaceParameters(List<int> ids = null)
    {
        var query = _context.FIASCarPlaceParameters
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<CarPlaceParameter> GetCarPlaceParameter(int id)
    {
        var query = _context.FIASCarPlaceParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> CarPlaceParameterExists(int id)
    {
        var query = _context.FIASCarPlaceParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddCarPlaceParameter(CarPlaceParameter carPlaceParameter)
    {
        _context.Entry(carPlaceParameter).State = EntityState.Added;
    }

    public void UpdateCarPlaceParameter(CarPlaceParameter carPlaceParameter)
    {
        _context.Entry(carPlaceParameter).State = EntityState.Modified;
    }

    public void RemoveCarPlaceParameter(CarPlaceParameter carPlaceParameter)
    {
        _context.Entry(carPlaceParameter).State = EntityState.Deleted;
    }

    #endregion
    
    #region House

    public async Task<List<House>> GetHouses(List<int> ids = null)
    {
        var query = _context.FIASHouses
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<House> GetHouse(int id)
    {
        var query = _context.FIASHouses
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> HouseExists(int id)
    {
        var query = _context.FIASHouses
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddHouse(House house)
    {
        _context.Entry(house).State = EntityState.Added;
    }

    public void UpdateHouse(House house)
    {
        _context.Entry(house).State = EntityState.Modified;
    }

    public void RemoveHouse(House house)
    {
        _context.Entry(house).State = EntityState.Deleted;
    }

    #endregion
    
    #region HouseParameter

    public async Task<List<HouseParameter>> GetHouseParameters(List<int> ids = null)
    {
        var query = _context.FIASHouseParameters
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<HouseParameter> GetHouseParameter(int id)
    {
        var query = _context.FIASHouseParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> HouseParameterExists(int id)
    {
        var query = _context.FIASCarPlaceParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddHouseParameter(HouseParameter houseParameter)
    {
        _context.Entry(houseParameter).State = EntityState.Added;
    }

    public void UpdateHouseParameter(HouseParameter houseParameter)
    {
        _context.Entry(houseParameter).State = EntityState.Modified;
    }

    public void RemoveHouseParameter(HouseParameter houseParameter)
    {
        _context.Entry(houseParameter).State = EntityState.Deleted;
    }

    #endregion
    
    #region Room

    public async Task<List<Room>> GetRooms(List<int> ids = null)
    {
        var query = _context.FIASRooms
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<Room> GetRoom(int id)
    {
        var query = _context.FIASRooms
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> RoomExists(int id)
    {
        var query = _context.FIASRooms
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddRoom(Room room)
    {
        _context.Entry(room).State = EntityState.Added;
    }

    public void UpdateRoom(Room room)
    {
        _context.Entry(room).State = EntityState.Modified;
    }

    public void RemoveRoom(Room room)
    {
        _context.Entry(room).State = EntityState.Deleted;
    }

    #endregion
    
    #region RoomParameter

    public async Task<List<RoomParameter>> GetRoomParameters(List<int> ids = null)
    {
        var query = _context.FIASRoomParameters
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<RoomParameter> GetRoomParameter(int id)
    {
        var query = _context.FIASRoomParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> RoomParameterExists(int id)
    {
        var query = _context.FIASRoomParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddRoomParameter(RoomParameter roomParameter)
    {
        _context.Entry(roomParameter).State = EntityState.Added;
    }

    public void UpdateRoomParameter(RoomParameter roomParameter)
    {
        _context.Entry(roomParameter).State = EntityState.Modified;
    }

    public void RemoveRoomParameter(RoomParameter roomParameter)
    {
        _context.Entry(roomParameter).State = EntityState.Deleted;
    }

    #endregion
    
    #region Stead

    public async Task<List<Stead>> GetSteads(List<int> ids = null)
    {
        var query = _context.FIASSteads
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<Stead> GetStead(int id)
    {
        var query = _context.FIASSteads
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> SteadExists(int id)
    {
        var query = _context.FIASSteads
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddStead(Stead stead)
    {
        _context.Entry(stead).State = EntityState.Added;
    }

    public void UpdateStead(Stead stead)
    {
        _context.Entry(stead).State = EntityState.Modified;
    }

    public void RemoveStead(Stead stead)
    {
        _context.Entry(stead).State = EntityState.Deleted;
    }

    #endregion
    
    #region SteadParameter

    public async Task<List<SteadParameter>> GetSteadParameters(List<int> ids = null)
    {
        var query = _context.FIASSteadParameters
            .AsQueryable()
            .AsNoTracking();
        
        if (ids != null)
        {
            query = query.Where(e => ids.Contains(e.Id));
        }

        return await query.ToListAsync();
    }

    public async Task<SteadParameter> GetSteadParameter(int id)
    {
        var query = _context.FIASSteadParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> SteadParameterExists(int id)
    {
        var query = _context.FIASSteadParameters
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddSteadParameter(SteadParameter steadParameter)
    {
        _context.Entry(steadParameter).State = EntityState.Added;
    }

    public void UpdateSteadParameter(SteadParameter steadParameter)
    {
        _context.Entry(steadParameter).State = EntityState.Modified;
    }

    public void RemoveSteadParameter(SteadParameter steadParameter)
    {
        _context.Entry(steadParameter).State = EntityState.Deleted;
    }

    #endregion
}