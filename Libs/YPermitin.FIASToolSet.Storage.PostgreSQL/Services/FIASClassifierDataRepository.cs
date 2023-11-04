using Microsoft.EntityFrameworkCore;
using YPermitin.FIASToolSet.Storage.Core.Models.ClassifierData;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Services;

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
}