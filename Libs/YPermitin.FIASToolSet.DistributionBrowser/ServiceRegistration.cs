using Microsoft.Extensions.DependencyInjection;

namespace YPermitin.FIASToolSet.DistributionBrowser
{
    /// <summary>
    /// Регистрация сервисов
    /// </summary>
    public static class ServiceRegistration
    {
        public static void AddFIASDistributionBrowser(this IServiceCollection services)
        {
            services.AddTransient<IFIASDistributionBrowser, FIASDistributionBrowser>();
        }
    }
}
