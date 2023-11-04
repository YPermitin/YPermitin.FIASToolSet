using Microsoft.EntityFrameworkCore;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Services;

public class FIASInstallationManagerRepository : CommonRepository, IFIASInstallationManagerRepository
{
    public FIASInstallationManagerRepository(FIASToolSetServiceContext context) : base(context)
    {
    }

    public async Task<List<FIASVersionInstallation>> GetInstallations(Guid? statusId = null, Guid? typeId = null,
        bool includeDetails = false)
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

        if (includeDetails)
        {
            query = query
                .Include(e => e.FIASVersion).AsNoTracking()
                .Include(e => e.Status).AsNoTracking()
                .Include(e => e.InstallationType).AsNoTracking();
        }

        var result = await query
            .OrderBy(e => e.Created)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }

    public async Task<FIASVersionInstallation> GetInstallation(Guid id)
    {
        return await _context.FIASVersionInstallations
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<FIASVersionInstallation> GetLastInstallation()
    {
        var query = _context.FIASVersionInstallations
            .Where(e => e.Created == _context.FIASVersionInstallations.Max(e => e.Created))
            .AsQueryable()
            .AsNoTracking();

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
            .AsQueryable()
            .AsNoTracking();

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