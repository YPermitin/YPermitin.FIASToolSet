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
    
    public async Task<List<AddressObject>> GetAddressObjects()
    {
        var query = _context.FIASAddressObjects
            .AsQueryable()
            .AsNoTracking();

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
}