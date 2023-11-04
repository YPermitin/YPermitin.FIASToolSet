using YPermitin.FIASToolSet.Storage.Core.Models.Notifications;
using YPermitin.FIASToolSet.Storage.Core.Models.Versions;

namespace YPermitin.FIASToolSet.Storage.Core.Services
{
    public interface IFIASMaintenanceRepository
    {
        /// <summary>
        /// Получение информации о версии ФИАС её идентификатору
        /// </summary>
        /// <param name="versionId">Идентификатор версии</param>
        /// <returns>Версия ФИАС</returns>
        Task<FIASVersion> GetVersion(Guid versionId);

        /// <summary>
        /// Получение информации о последней версии ФИАС
        /// </summary>
        /// <returns>Версия ФИАС</returns>
        Task<FIASVersion> GetLastVersion(int? versionId = null);

        /// <summary>
        /// Получение информации о предыдущей версии ФИАС
        /// </summary>
        /// <param name="currentVersionId">Идентификатор версии ФИАС, от которой нужно получить предыдущую версию</param>
        /// <returns>Версия ФИАС</returns>
        Task<FIASVersion> GetPreviousVersion(Guid currentVersionId);

        /// <summary>
        /// Добавление новой версии ФИАС
        /// </summary>
        /// <param name="version">Версия ФИАС</param>
        /// <returns>Объект задачи асинхронной операциии</returns>
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
