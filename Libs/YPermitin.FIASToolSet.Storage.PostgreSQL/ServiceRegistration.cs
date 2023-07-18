using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YPermitin.FIASToolSet.Storage.Core.Services;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;
using YPermitin.FIASToolSet.Storage.PostgreSQL.Services;

namespace YPermitin.FIASToolSet.Storage.PostgreSQL
{
    public static class ServiceRegistration
    {
        public static void AddFIASStorageOnPostgreSQL(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<FIASToolSetServiceContext>(options =>
            {
                string connectionString = configuration.GetConnectionString("FIASToolSetService");
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IFIASMaintenanceRepository, FIASMaintenanceRepository>();
            services.AddScoped<IFIASInstallationManagerRepository, FIASInstallationManagerRepository>();
            services.AddScoped<IFIASBaseCatalogsRepository, FIASBaseCatalogsRepository>();
        }
    }
}
