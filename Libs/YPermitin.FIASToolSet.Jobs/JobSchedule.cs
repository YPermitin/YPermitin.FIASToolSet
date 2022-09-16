namespace YPermitin.FIASToolSet.Jobs
{
    /// <summary>
    /// Настройка планировщика для задания
    /// </summary>
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        /// <summary>
        /// Тип задания
        /// </summary>
        public Type JobType { get; }

        /// <summary>
        /// CRON-выражение расписания запуска
        /// </summary>
        public string CronExpression { get; }
    }
}
