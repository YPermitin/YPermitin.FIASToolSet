using Microsoft.EntityFrameworkCore;
using YPermitin.FIASToolSet.Storage.Core.Models;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.SQLServer.DbContexts;

namespace YPermitin.FIASToolSet.Storage.SQLServer.Services;

public class FIASInstallationManagerRepository : CommonRepository, IFIASInstallationManagerRepository
{
    public FIASInstallationManagerRepository(FIASToolSetServiceContext context) : base(context)
    {
    }

    public async Task<List<FIASVersionInstallation>> GetInstallations(Guid? statusId = null, Guid? typeId = null)
    {
        var query = _context.FIASVersionInstallations.AsQueryable();

        if (statusId != null)
        {
            query = query.Where(e => e.StatusId == statusId);
        }
        
        if (typeId != null)
        {
            query = query.Where(e => e.InstallationTypeId == typeId);
        }

        var result = await query.OrderBy(e => e.Created).ToListAsync();

        return result;
    }

    public async Task<FIASVersionInstallation> GetInstallation(Guid id)
    {
        return await _context.FIASVersionInstallations
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<FIASVersionInstallation> GetLastInstallation()
    {
        var query = _context.FIASVersionInstallations
            .Where(e => e.Created == _context.FIASVersionInstallations.Max(e => e.Created))
            .AsQueryable();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<FIASVersionInstallation> GetPreviousInstallation(Guid installationId)
    {
        var lastVersionQuery = _context.FIASVersionInstallations
            .AsNoTracking()
            .Where(lv => lv.Id != installationId
                         && lv.Created <= _context.FIASVersionInstallations
                             .Where(lvm => lvm.Id == installationId)
                             .Max(lvm => lvm.Created)
            )
            .OrderByDescending(lv => lv.Created)
            .AsQueryable();

        var lastVersion = await lastVersionQuery.FirstOrDefaultAsync();

        return lastVersion;
    }

    public void AddInstallation(FIASVersionInstallation installation)
    {
        _context.Entry(installation).State = EntityState.Added;
    }

    public void UpdateInstallation(FIASVersionInstallation installation)
    {
        _context.Entry(installation).State = EntityState.Modified;
    }
}