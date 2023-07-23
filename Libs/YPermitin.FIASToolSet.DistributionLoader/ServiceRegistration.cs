using Microsoft.Extensions.DependencyInjection;

namespace YPermitin.FIASToolSet.DistributionLoader;

public static class ServiceRegistration
{
    public static void AddFIASDistributionLoader(this IServiceCollection services)
    {
        services.AddScoped<IFIASDistributionLoader, FIASDistributionLoader>();
    }
}