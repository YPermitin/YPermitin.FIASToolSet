using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YPermitin.FIASToolSet.Storage.Core.Models;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.SQLServer.DbContexts;

namespace YPermitin.FIASToolSet.Storage.SQLServer.Services
{
    public class FIASMaintenanceRepository : CommonRepository, IFIASMaintenanceRepository
    { 
        public FIASMaintenanceRepository(FIASToolSetServiceContext context) : base(context)
        {
        }

        public async Task<FIASVersion> GetLastVersion()
        {
            var lastVersionQuery = _context.FIASVersions
                .AsNoTracking()
                .Where(lv => lv.Period == _context.FIASVersions.Max(lvm => lvm.Period))
                .AsQueryable();

            var lastVersion = await lastVersionQuery.FirstOrDefaultAsync();

            return lastVersion;
        }

        public async Task AddVersion(FIASVersion version)
        {
            await _context.AddAsync(version);
        }

        public async Task AddNotification(NotificationQueue notificationQueueItem)
        {
            await _context.AddAsync(notificationQueueItem);
        }

        public void UpdateNotification(NotificationQueue notificationQueueItem)
        {
            _context.Update(notificationQueueItem);
        }

        public async Task<List<NotificationQueue>> GetNotifications(Guid notificationStatusId, int limit = 10)
        {
            return await _context.NotificationsQueues
                .AsNoTracking()
                .Where(ns => ns.StatusId == notificationStatusId)
                .OrderBy(ns => ns.Period)
                .Take(limit)
                .ToListAsync();
        }
    }
}
