using Microsoft.Extensions.DependencyInjection;

namespace YPermitin.FIASToolSet.Loader
{
    /// <summary>
    /// Регистрация сервисов
    /// </summary>
    public static class ServiceRegistration
    {
        public static void AddFIASLoader(this IServiceCollection services)
        {
            services.AddTransient<IFIASLoader, FIASLoader>();
        }
    }
}
