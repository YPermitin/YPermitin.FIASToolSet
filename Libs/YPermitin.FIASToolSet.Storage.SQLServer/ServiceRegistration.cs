using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.SQLServer.DbContexts;
using YPermitin.FIASToolSet.Storage.SQLServer.Services;

namespace YPermitin.FIASToolSet.Storage.SQLServer
{
    public static class ServiceRegistration
    {
        public static void AddFIASStorageOnSQLServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FIASToolSetServiceContext>(options =>
            {
                string connectionString = configuration.GetConnectionString("FIASToolSetService");
                options.UseSqlServer(connectionString);
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            services.AddScoped<IFIASMaintenanceRepository, FIASMaintenanceRepository>();
            services.AddScoped<IFIASInstallationManagerRepository, FIASInstallationManagerRepository>();
            services.AddScoped<IFIASBaseCatalogsRepository, FIASBaseCatalogsRepository>();
            services.AddScoped<IFIASClassifierDataRepository, FIASClassifierDataRepository>();
        }
        
        public static void UseFIASStorageOnSQLServer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<FIASToolSetServiceContext>();

                dbContext.Database.Migrate();
            }
        }
    }
}
