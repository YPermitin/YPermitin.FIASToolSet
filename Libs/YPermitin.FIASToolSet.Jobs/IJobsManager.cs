using Quartz;
using YPermitin.FIASToolSet.Jobs.Models;

namespace YPermitin.FIASToolSet.Jobs
{
    /// <summary>
    /// Управление планировщиком заданий в части состава заданий и их настроек
    /// </summary>
    public interface IJobsManager
    {
        /// <summary>
        /// Формирование списка заданий
        /// </summary>
        /// <param name="activeOnly">Только активные задания</param>
        /// <returns>Список заданий с доп. атрибутами</returns>
        Task<IReadOnlyCollection<JobInfo>> GetAllJobs(bool activeOnly = false);

        /// <summary>
        /// Создание задания
        /// </summary>
        /// <typeparam name="T">Тип класса задания</typeparam>
        /// <param name="jobInfo">Настройки задания</param>
        /// <returns>Объект асинхронной операции</returns>
        Task CreateJobItem<T>(JobInfoWithDetails jobInfo)
            where T : IJob;

        /// <summary>
        /// Создание задания
        /// </summary>
        /// <typeparam name="T">Тип класса задания</typeparam>
        /// <param name="jobInfo">Настройки задания</param>
        /// <param name="cancellationToken">Токен асинхронной операции</param>
        /// <returns>Объект асинхронной операции</returns>
        Task CreateJobItem<T>(JobInfoWithDetails jobInfo, CancellationToken cancellationToken)
            where T : IJob;

        /// <summary>
        /// Обновление задания
        /// </summary>
        /// <typeparam name="T">Тип класса задания</typeparam>
        /// <param name="jobInfo">Настройки задания</param>
        /// <returns>Объект асинхронной операции</returns>
        Task UpdateJobItem<T>(JobInfoWithDetails jobInfo)
            where T : IJob;

        /// <summary>
        /// Обновление задания
        /// </summary>
        /// <typeparam name="T">Тип класса задания</typeparam>
        /// <param name="jobInfo">Настройки задания</param>
        /// <param name="cancellationToken">Токен асинхронной операции</param>
        /// <returns>Объект асинхронной операции</returns>
        Task UpdateJobItem<T>(JobInfoWithDetails jobInfo, CancellationToken cancellationToken)
            where T : IJob;

        /// <summary>
        /// Удаление задания
        /// </summary>
        /// <param name="jobKey">Ключ задания</param>
        /// <param name="cancellationToken">Токен асинхронной операции</param>
        /// <returns>Объект асинхронной операции</returns>
        Task DeleteJobItem(string jobKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Прерывание выполнения задания
        /// </summary>
        /// <param name="jobKeyName">Ключ задания</param>
        /// <param name="cancellationToken">Токен асинхронной операции</param>
        /// <returns>Объект асинхронной операции</returns>
        Task InterruptJobItem(string jobKeyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Инициализация планировщика по умолчанию
        /// </summary>
        /// <returns>Объект асинхронной операции</returns>
        Task InitDefaultScheduler();
    }
}
