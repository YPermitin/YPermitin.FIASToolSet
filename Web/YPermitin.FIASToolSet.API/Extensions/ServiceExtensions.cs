using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using YPermitin.FIASToolSet.API.Models;

namespace YPermitin.FIASToolSet.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void UseExceptionPage(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }
        }

        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("FIASToolSetServiceAPISpecification", new OpenApiInfo()
                {
                    Title = "FIASToolSet service API",
                    Version = "1",
                    Description = "FIASToolSet service API - набор инструменов для работы с классификатором ФИАС.",
                    Contact = new OpenApiContact()
                    {
                        Email = "i.need.ypermitin@yandex.ru",
                        Name = "Permitin Yuriy (Пермитин Юрий)",
                        Url = new Uri("https://github.com/YPermitin")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Лицензия (MIT)",
                        Url = new Uri("https://github.com/YPermitin/YPermitin.FIASToolSet/blob/main/LICENSE")
                    }
                });

                var xmlDocFolder = AppContext.BaseDirectory;
                var xmlDocFiles = Directory.GetFiles(xmlDocFolder, "*.xml");
                foreach (var xmlDocFile in xmlDocFiles)
                {
                    FileInfo xmlDocFileInfo = new FileInfo(xmlDocFile);
                    if (xmlDocFileInfo.Name.StartsWith("YPermitin.FIASToolSet."))
                    {
                        setupAction.IncludeXmlComments(xmlDocFileInfo.FullName);
                    }
                }

                setupAction.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                setupAction.DocInclusionPredicate((_, _) => true);
            });
        }

        public static void UseSwaggerExtension(this IApplicationBuilder app, Type assemblyType)
        {
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.InjectStylesheet("/Assets/api/custom-ui.css");
                setupAction.IndexStream = ()
                    => assemblyType.Assembly
                        .GetManifestResourceStream("YPermitin.FIASToolSet.API.EmbeddedAssets.API.index.html");

                setupAction.SwaggerEndpoint("/swagger/FIASToolSetServiceAPISpecification/swagger.json",
                    "FIASToolSet service API");
                setupAction.RoutePrefix = string.Empty;

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(ModelRendering.Model);
                setupAction.DocExpansion(DocExpansion.None);
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }

        public static void AddControllersExtension(this IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetailsFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ProblemDetailsFactory>();
                        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext,
                            context.ModelState);

                        problemDetails.Detail = "See the errors field for details.";
                        problemDetails.Instance = context.HttpContext.Request.Path;

                        var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                        if (context.ModelState.ErrorCount > 0 &&
                            (context is ControllerContext ||
                             actionExecutingContext?.ActionArguments.Count ==
                             context.ActionDescriptor.Parameters.Count))
                        {
                            problemDetails.Type = "https://FIASToolSet.ru/modelvalidationproblem";
                            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetails.Title = "One or more validation errors occurred.";

                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes = { "application/problem+json" }
                            };
                        }

                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "One or more errors on input occurred.";
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });
        }

        public static void AddMVCExtension(this IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                // Отключен список стандартных ответов для методов API
                //setupAction.Filters.Add(
                //    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                //setupAction.Filters.Add(
                //    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                //setupAction.Filters.Add(
                //    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                //setupAction.Filters.Add(
                //    new ProducesDefaultResponseTypeAttribute());
                //setupAction.Filters.Add(
                //    new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));

                setupAction.ReturnHttpNotAcceptable = true;
                var stringFormatter = setupAction.OutputFormatters.FirstOrDefault(f => f is StringOutputFormatter);
                if (stringFormatter != null) setupAction.OutputFormatters.Remove(stringFormatter);

                setupAction.InputFormatters.Add(new XmlSerializerInputFormatter(setupAction));
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });
        }
    }
}
