using Microsoft.Extensions.DependencyInjection;

namespace YPermitin.FIASToolSet.Jobs.Extensions
{
    /// <summary>
    /// Дополнительные методы управления заданиями
    /// </summary>
    public static class JobExtensions
    {
        /// <summary>
        /// Регистрирует задание Quarts.NET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="cronSchedule">Расписание запуска в формате Cron</param>
        public static void RegisterJob<T>(this IServiceCollection services, string cronSchedule)
            where T : class
        {
            services.AddSingleton<T>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(T),
                cronExpression: cronSchedule));
        }

        /// <summary>
        /// Регистрирует задание Quarts.NET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services">Коллекция сервисов</param>
        public static void RegisterJob<T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<T>();
        }
    }
}
