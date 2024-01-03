using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YPermitin.FIASToolSet.Storage.ClickHouse
{
    public static class ServiceRegistration
    {
        public static void AddFIASStorageOnClickHouse(this IServiceCollection services, IConfiguration configuration)
        {
            

            /*services.AddScoped<IFIASMaintenanceRepository, FIASMaintenanceRepository>();
            services.AddScoped<IFIASInstallationManagerRepository, FIASInstallationManagerRepository>();
            services.AddScoped<IFIASBaseCatalogsRepository, FIASBaseCatalogsRepository>();
            services.AddScoped<IFIASClassifierDataRepository, FIASClassifierDataRepository>();*/
        }
        
        public static void UseFIASStorageOnClickHouse(this IApplicationBuilder app)
        {
            /*using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<FIASToolSetServiceContext>();

                dbContext.Database.Migrate();
            }*/
        }
    }
}
