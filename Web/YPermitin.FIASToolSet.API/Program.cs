using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using YPermitin.FIASToolSet.API.Extensions;
using YPermitin.FIASToolSet.API.Infrastructure;
using YPermitin.FIASToolSet.DistributionBrowser;
using YPermitin.FIASToolSet.Jobs;
using YPermitin.FIASToolSet.Storage.PostgreSQL;

namespace YPermitin.FIASToolSet.API
{
    public class Program
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
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

            string logPath = Path.Combine("Logs", "log-.txt");
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
                .WriteTo.Console()
                .CreateLogger();

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
                else
                {
                    throw new Exception($"Unknown service deploy type in config file: {serviceDeployType}");
                }

                builder.Host.UseSerilog((_, lc) => lc
                    .ReadFrom.Configuration(Configuration)
                    .WriteTo.Console()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7));
                var services = builder.Services;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
                services.AddCors(policy =>
                {
                    string[] allowedOrigins = Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();

                    policy.AddPolicy("webAppAllowSpecificOrigins", corsPolicyBuilder =>
                    {
                        corsPolicyBuilder
                            .WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
                });

                services.AddFIASDistributionBrowser();
                services.AddFIASStorageOnPostgreSQL(Configuration);

                services.AddControllersExtension();
                services.AddMVCExtension();
                services.AddHttpContextAccessor();
                services.AddSwaggerExtension();
                services.AddJobsService(Configuration);

                var app = builder.Build();
                IWebHostEnvironment env = app.Environment;
                var logger = app.Services.GetService<ILogger<Program>>();

                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });

                app.UseExceptionPage(env);
                app.ConfigureExceptionHandler(logger);
                app.UseHttpsRedirection();
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors("webAppAllowSpecificOrigins");

                app.UseSwagger();
                app.UseSwaggerExtension(typeof(Program));

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                app.Run();
            }
            catch (Exception ex)
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