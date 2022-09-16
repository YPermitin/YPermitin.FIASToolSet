using YPermitin.FIASToolSet.Storage.Core.Models;

namespace YPermitin.FIASToolSet.Storage.Core.Services
{
    public interface IFIASMaintenanceRepository
    {
        Task<FIASVersion> GetLastVersion();
        Task AddVersion(FIASVersion version);
        Task AddNotification(NotificationQueue notificationQueueItem);
        void UpdateNotification(NotificationQueue notificationQueueItem);
        Task<List<NotificationQueue>> GetNotifications(Guid notificationStatusId, int limit = 10);

        Task<bool> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<bool> SaveAsync();
    }
}
