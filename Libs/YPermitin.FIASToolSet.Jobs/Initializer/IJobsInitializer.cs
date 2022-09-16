namespace YPermitin.FIASToolSet.Jobs.Initializer
{
    /// <summary>
    /// Объет инициализации заданий
    /// </summary>
    public interface IJobsInitializer
    {
        /// <summary>
        /// Запуск / перезаупск заданий
        /// </summary>
        /// <returns>Объект асинхронной операции</returns>
        Task StartOrRestartAllJob();

        /// <summary>
        /// Остановить задания
        /// </summary>
        /// <returns>Объект асинхронной операции</returns>
        Task StopAllJob();
    }
}
