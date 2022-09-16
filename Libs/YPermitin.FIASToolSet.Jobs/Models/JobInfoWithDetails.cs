namespace YPermitin.FIASToolSet.Jobs.Models
{
    /// <summary>
    /// Информация о задании с доп. информацией
    /// </summary>
    public class JobInfoWithDetails
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string Key { get; set; } = null!;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// CRON-выражение расписания запуска
        /// </summary>
        public string CronExpression { get; set; } = null!;

        /// <summary>
        /// Приоритет (чем ниже значение, тем выше приоритет)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Настройки работы задания
        /// </summary>
        public JobSettings Settings { get; set; } = null!;
    }
}
