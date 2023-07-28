using Microsoft.EntityFrameworkCore;
using YPermitin.FIASToolSet.Storage.Core.Models;
using YPermitin.FIASToolSet.Storage.Core.Models.BaseCatalogs;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Services;

public class FIASBaseCatalogsRepository: CommonRepository, IFIASBaseCatalogsRepository
{
    public FIASBaseCatalogsRepository(FIASToolSetServiceContext context) : base(context)
    {
    }

    #region ObjectLevel

    public async Task<List<ObjectLevel>> GetObjectLevels()
    {
        var query = _context.FIASObjectLevels
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<ObjectLevel> GetObjectLevel(int level)
    {
        var query = _context.FIASObjectLevels
            .Where(e => e.Level == level)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> ObjectLevelExists(int level)
    {
        var query = _context.FIASObjectLevels
            .Where(e => e.Level == level)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddObjectLevel(ObjectLevel objectLevel)
    {
        _context.Entry(objectLevel).State = EntityState.Added;
    }

    public void UpdateObjectLevel(ObjectLevel objectLevel)
    {
        _context.Entry(objectLevel).State = EntityState.Modified;
    }

    public void RemoveObjectLevel(ObjectLevel objectLevel)
    {
        _context.Entry(objectLevel).State = EntityState.Deleted;
    }
    
    #endregion

    #region AddressObjectType
    
    public async Task<List<AddressObjectType>> GetAddressObjectTypes()
    {
        var query = _context.FIASAddressObjectTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<AddressObjectType> GetAddressObjectType(int id)
    {
        var query = _context.FIASAddressObjectTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> AddressObjectTypeExists(int id)
    {
        var query = _context.FIASAddressObjectTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddAddressObjectType(AddressObjectType addressObjectType)
    {
        _context.Entry(addressObjectType).State = EntityState.Added;
    }

    public void UpdateAddressObjectType(AddressObjectType addressObjectType)
    {
        _context.Entry(addressObjectType).State = EntityState.Modified;
    }

    public void RemoveAddressObjectType(AddressObjectType addressObjectType)
    {
        _context.Entry(addressObjectType).State = EntityState.Deleted;
    }
    
    #endregion

    #region ApartmentType
    
    public async Task<List<ApartmentType>> GetApartmentTypes()
    {
        var query = _context.FIASApartmentTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<ApartmentType> GetApartmentType(int id)
    {
        var query = _context.FIASApartmentTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> ApartmentTypeExists(int id)
    {
        var query = _context.FIASApartmentTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddApartmentType(ApartmentType apartmentType)
    {
        _context.Entry(apartmentType).State = EntityState.Added;
    }

    public void UpdateApartmentType(ApartmentType apartmentType)
    {
        _context.Entry(apartmentType).State = EntityState.Modified;
    }

    public void RemoveApartmentType(ApartmentType apartmentType)
    {
        _context.Entry(apartmentType).State = EntityState.Deleted;
    }
    
    #endregion

    #region HouseType
    
    public async Task<List<HouseType>> GetHouseTypes()
    {
        var query = _context.FIASHouseTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<HouseType> GetHouseType(int id)
    {
        var query = _context.FIASHouseTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> HouseTypeExists(int id)
    {
        var query = _context.FIASHouseTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddHouseType(HouseType houseType)
    {
        _context.Entry(houseType).State = EntityState.Added;
    }

    public void UpdateHouseType(HouseType houseType)
    {
        _context.Entry(houseType).State = EntityState.Modified;
    }

    public void RemoveHouseType(HouseType houseType)
    {
        _context.Entry(houseType).State = EntityState.Deleted;
    }
    
    #endregion

    #region NormativeDocKind
    
    public async Task<List<NormativeDocKind>> GetNormativeDocKinds()
    {
        var query = _context.FIASNormativeDocKinds
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<NormativeDocKind> GetNormativeDocKind(int id)
    {
        var query = _context.FIASNormativeDocKinds
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> NormativeDocKindExists(int id)
    {
        var query = _context.FIASNormativeDocKinds
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddNormativeDocKind(NormativeDocKind normativeDocKind)
    {
        _context.Entry(normativeDocKind).State = EntityState.Added;
    }

    public void UpdateNormativeDocKind(NormativeDocKind normativeDocKind)
    {
        _context.Entry(normativeDocKind).State = EntityState.Modified;
    }

    public void RemoveNormativeDocKind(NormativeDocKind normativeDocKind)
    {
        _context.Entry(normativeDocKind).State = EntityState.Deleted;
    }
    
    #endregion

    #region NormativeDocType
    
    public async Task<List<NormativeDocType>> GetNormativeDocTypes()
    {
        var query = _context.FIASNormativeDocTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<NormativeDocType> GetNormativeDocType(int id)
    {
        var query = _context.FIASNormativeDocTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> NormativeDocTypeExists(int id)
    {
        var query = _context.FIASNormativeDocTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddNormativeDocType(NormativeDocType normativeDocType)
    {
        _context.Entry(normativeDocType).State = EntityState.Added;
    }

    public void UpdateNormativeDocType(NormativeDocType normativeDocType)
    {
        _context.Entry(normativeDocType).State = EntityState.Modified;
    }

    public void RemoveNormativeDocType(NormativeDocType normativeDocType)
    {
        _context.Entry(normativeDocType).State = EntityState.Deleted;
    }
    
    #endregion

    #region OperationType
    
    public async Task<List<OperationType>> GetOperationTypes()
    {
        var query = _context.FIASOperationTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<OperationType> GetOperationType(int id)
    {
        var query = _context.FIASOperationTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> OperationTypeExists(int id)
    {
        var query = _context.FIASOperationTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddOperationType(OperationType operationType)
    {
        _context.Entry(operationType).State = EntityState.Added;
    }

    public void UpdateOperationType(OperationType operationType)
    {
        _context.Entry(operationType).State = EntityState.Modified;
    }

    public void RemoveOperationType(OperationType operationType)
    {
        _context.Entry(operationType).State = EntityState.Deleted;
    }
    
    #endregion

    #region ParameterType

    public async Task<List<ParameterType>> GetParameterTypes()
    {
        var query = _context.FIASParameterTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<ParameterType> GetParameterType(int id)
    {
        var query = _context.FIASParameterTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> ParameterTypeExists(int id)
    {
        var query = _context.FIASParameterTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddParameterType(ParameterType parameterType)
    {
        _context.Entry(parameterType).State = EntityState.Added;
    }

    public void UpdateParameterType(ParameterType parameterType)
    {
        _context.Entry(parameterType).State = EntityState.Modified;
    }

    public void RemoveParameterType(ParameterType parameterType)
    {
        _context.Entry(parameterType).State = EntityState.Deleted;
    }
    
    #endregion

    #region RoomType
    
    public async Task<List<RoomType>> GetRoomTypes()
    {
        var query = _context.FIASRoomTypes
            .AsQueryable()
            .AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<RoomType> GetRoomType(int id)
    {
        var query = _context.FIASRoomTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> RoomTypeExists(int id)
    {
        var query = _context.FIASRoomTypes
            .Where(e => e.Id == id)
            .AsQueryable()
            .AsNoTracking();

        return await query.AnyAsync();
    }

    public void AddRoomType(RoomType roomType)
    {
        _context.Entry(roomType).State = EntityState.Added;
    }

    public void UpdateRoomType(RoomType roomType)
    {
        _context.Entry(roomType).State = EntityState.Modified;
    }

    public void RemoveRoomType(RoomType roomType)
    {
        _context.Entry(roomType).State = EntityState.Deleted;
    }
    
    #endregion
}