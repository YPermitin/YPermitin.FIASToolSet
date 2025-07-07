using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;
using System.Text;
using YPermitin.FIASToolSet.API.Extensions;
using YPermitin.FIASToolSet.API.Infrastructure;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.DistributionLoader;
using YPermitin.FIASToolSet.Jobs;
using YPermitin.FIASToolSet.Storage.ClickHouse;
using YPermitin.FIASToolSet.Storage.PostgreSQL;
using YPermitin.FIASToolSet.Storage.SQLServer;

namespace YPermitin.FIASToolSet.API
{
    public class Program
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        public static void Main(string[] args)
        {
            ServiceDeployType serviceDeployTypeDefault = ServiceDeployType.Unknown;
            if (OperatingSystem.IsWindows())
            {
                serviceDeployTypeDefault = ServiceDeployType.IIS;
            }
            else if (OperatingSystem.IsLinux())
            {
                serviceDeployTypeDefault = ServiceDeployType.Kestrel;
            }
            string serviceDeployTypeAsString = Configuration.GetValue("DeployType", string.Empty);
            var serviceDeployType = serviceDeployTypeAsString.ToEnum(serviceDeployTypeDefault);

            string logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt");
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
                .Enrich.FromLogContext()
                .CreateLogger();
            Log.Logger = logger;

            try
            {
                Log.Information("Starting web host");

                WebApplicationBuilder builder;
                if (serviceDeployType == ServiceDeployType.WindowsService)
                {
                    WebApplicationOptions webApplicationOptions = new()
                    {
                        ContentRootPath = AppContext.BaseDirectory,
                        Args = args,
                        ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                    };
                    builder = WebApplication.CreateBuilder(webApplicationOptions);
                }
                else
                {
                    builder = WebApplication.CreateBuilder(args);
                }

                if (serviceDeployType == ServiceDeployType.IIS)
                {
                    builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
                    builder.WebHost.UseIISIntegration();
                }
                else if (serviceDeployType == ServiceDeployType.Kestrel)
                {
                    builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
                    builder.WebHost.UseKestrel();
                }
                else if (serviceDeployType == ServiceDeployType.WindowsService)
                {
                    if (OperatingSystem.IsWindows())
                    {
                        builder.WebHost.UseKestrel();
                        builder.Host.UseWindowsService();
                    }
                    else
                    {
                        throw new NotSupportedException("Windows Service only runs on Windows OS.");
                    }
                }
                else
                {
                    throw new Exception($"Unknown service deploy type in config file: {serviceDeployType}");
                }

                builder.Logging.ClearProviders();
#if DEBUG
                builder.Logging.AddConsole();
#endif
                builder.Logging.AddSerilog(logger);

                var services = builder.Services;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
                services.AddCors(policy =>
                {
                    string[] allowedOrigins = Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();

                    if (allowedOrigins == null || allowedOrigins.Length == 0)
                    {
                        allowedOrigins = new List<string>()
                        {
                            "http://localhost"
                        }.ToArray();
                    }

                    policy.AddPolicy("webAppAllowSpecificOrigins", corsPolicyBuilder =>
                    {
                        corsPolicyBuilder
                            .WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
                });

                services.AddFIASDistributionBrowser(Configuration);

                var dbmsType = Configuration.GetValue("DBMSType", "PostgreSQL");
                var dbmsTypeValue = dbmsType.ToEnum(DBMSType.PostgreSQL);
                switch (dbmsTypeValue)
                {
                    case DBMSType.PostgreSQL:
                        services.AddFIASStorageOnPostgreSQL(Configuration);
                        break;
                    case DBMSType.SQLServer:
                        services.AddFIASStorageOnSQLServer(Configuration);
                        break;
                    case DBMSType.ClickHouse:
                        services.AddFIASStorageOnClickHouse(Configuration);
                        break;
                    default:
                        throw new Exception($"Unknown DBMS type for service database: {dbmsType}");
                }

                services.AddFIASDistributionLoader();

                services.AddControllersExtension();
                services.AddMVCExtension();
                services.AddHttpContextAccessor();
                services.AddSwaggerExtension();
                services.AddJobsService(Configuration);

                var app = builder.Build();
                IWebHostEnvironment env = app.Environment;
                var loggerObject = app.Services.GetService<ILogger<Program>>();

                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });

                app.UseExceptionPage(env);
                app.ConfigureExceptionHandler(loggerObject);
                
                switch (dbmsTypeValue)
                {
                    case DBMSType.PostgreSQL:
                        app.UseFIASStorageOnPostgreSQL();
                        break;
                    case DBMSType.SQLServer:
                        app.UseFIASStorageOnSQLServer();
                        break;
                    case DBMSType.ClickHouse:
                        app.UseFIASStorageOnClickHouse();
                        break;
                    default:
                        throw new Exception($"Unknown DBMS type for service database: {dbmsType}");
                }
                
                app.UseHttpsRedirection();
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors("webAppAllowSpecificOrigins");

                app.UseSwagger();
                app.UseSwaggerExtension(typeof(Program));

                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

                app.Run();
            }
            catch (Exception ex) 
                when(ex is not OperationCanceledException && ex.GetType().Name != "HostAbortedException")
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}