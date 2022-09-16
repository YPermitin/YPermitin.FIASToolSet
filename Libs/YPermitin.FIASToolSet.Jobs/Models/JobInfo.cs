namespace YPermitin.FIASToolSet.Jobs.Models
{
    /// <summary>
    /// Задание
    /// </summary>
    public class JobInfo
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string JobKey { get; set; } = null!;

        /// <summary>
        /// Наименование
        /// </summary>
        public string JobName { get; set; } = null!;

        /// <summary>
        /// CRON-выражение расписания запуска
        /// </summary>
        public string CronExpression { set; get; } = null!;

        /// <summary>
        /// Дата последнего запуска
        /// </summary>
        public DateTimeOffset? PreviousFireTime { get; set; }

        /// <summary>
        /// Дата последнего запуска
        /// </summary>
        public DateTimeOffset? NextFireTime { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
