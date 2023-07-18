using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YPermitin.FIASToolSet.DistributionBrowser.Models;

namespace YPermitin.FIASToolSet.DistributionBrowser
{
    /// <summary>
    /// Регистрация сервисов
    /// </summary>
    public static class ServiceRegistration
    {
        public static void AddFIASDistributionBrowser(this IServiceCollection services, IConfiguration configuration)
        {
            string generalWorkingDirectory = configuration.GetValue("FIASToolSet:WorkingDirectory", string.Empty);
            long maxDownloadSpeed = configuration.GetValue("FIASToolSet:MaximumDownloadSpeedBytesPerSecond", long.MaxValue);

            var browserOptions = new FIASDistributionBrowserOptions(generalWorkingDirectory, maxDownloadSpeed);
            services.AddTransient<IFIASDistributionBrowser, FIASDistributionBrowser>(_ =>
            {
                return new FIASDistributionBrowser(browserOptions);
            });
        }
    }
}
