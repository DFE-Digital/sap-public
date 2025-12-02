using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Workforce;
using SAPPub.Core.Services;
using SAPPub.Core.Services.KS4.Absence;
using SAPPub.Core.Services.KS4.Destinations;
using SAPPub.Core.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.Workforce;
using SAPPub.Infrastructure.Repositories;
using SAPPub.Infrastructure.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Absence;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;
using SAPPub.Infrastructure.Repositories.KS4.Performance;
using SAPPub.Infrastructure.Repositories.KS4.Workforce;
using SAPPub.Web.Helpers;
using SAPPub.Web.Middleware;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;

namespace SAPPub.Web;

public partial class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGovUkFrontend(options =>
        {
            options.Rebrand = true;
        });

        // Single, consolidated controllers registration
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

        // Add Razor pages separately if needed
        builder.Services.AddRazorPages();

        // View configuration
        builder.Services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/{0}.cshtml");
        });

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
            options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-GB") };
            options.RequestCultureProviders.Clear();
        });

        builder.Services.AddHealthChecks();
        builder.Services.AddDataProtection()
               .PersistKeysToFileSystem(new DirectoryInfo(@"/keys"))
               .SetApplicationName("SAPPub");


        //IServiceCollection serviceCollection =

        builder.Services.AddSingleton<IGenericRepository<Establishment>, JSONRepository<Establishment>>();
        builder.Services.AddSingleton<IEstablishmentRepository, EstablishmentRepository>();
        builder.Services.AddSingleton<IEstablishmentService, EstablishmentService>();

        builder.Services.AddSingleton<IGenericRepository<EstablishmentMetadata>, JSONRepository<EstablishmentMetadata>>();
        builder.Services.AddSingleton<IEstablishmentMetadataRepository, EstablishmentMetadataRepository>();
        builder.Services.AddSingleton<IEstablishmentMetadataService, EstablishmentMetadataService>();

        builder.Services.AddSingleton<IGenericRepository<EstablishmentPerformance>, JSONRepository<EstablishmentPerformance>>();
        builder.Services.AddSingleton<IEstablishmentPerformanceRepository, EstablishmentPerformanceRepository>();
        builder.Services.AddSingleton<IEstablishmentPerformanceService, EstablishmentPerformanceService>();

        builder.Services.AddSingleton<IGenericRepository<EstablishmentDestinations>, JSONRepository<EstablishmentDestinations>>();
        builder.Services.AddSingleton<IEstablishmentDestinationsRepository, EstablishmentDestinationsRepository>();
        builder.Services.AddSingleton<IEstablishmentDestinationsService, EstablishmentDestinationsService>();

        builder.Services.AddSingleton<IGenericRepository<EstablishmentAbsence>, JSONRepository<EstablishmentAbsence>>();
        builder.Services.AddSingleton<IEstablishmentAbsenceRepository, EstablishmentAbsenceRepository>();
        builder.Services.AddSingleton<IEstablishmentAbsenceService, EstablishmentAbsenceService>();

        builder.Services.AddSingleton<IGenericRepository<EstablishmentWorkforce>, JSONRepository<EstablishmentWorkforce>>();
        builder.Services.AddSingleton<IEstablishmentWorkforceRepository, EstablishmentWorkforceRepository>();
        builder.Services.AddSingleton<IEstablishmentWorkforceService, EstablishmentWorkforceService>();

        builder.Services.AddSingleton<IGenericRepository<LAPerformance>, JSONRepository<LAPerformance>>();
        builder.Services.AddSingleton<ILAPerformanceRepository, LAPerformanceRepository>();
        builder.Services.AddSingleton<ILAPerformanceService, LAPerformanceService>();

        builder.Services.AddSingleton<IGenericRepository<LADestinations>, JSONRepository<LADestinations>>();
        builder.Services.AddSingleton<ILADestinationsRepository, LADestinationsRepository>();
        builder.Services.AddSingleton<ILADestinationsService, LADestinationsService>();

        builder.Services.AddSingleton<IGenericRepository<EnglandPerformance>, JSONRepository<EnglandPerformance>>();
        builder.Services.AddSingleton<IEnglandPerformanceRepository, EnglandPerformanceRepository>();
        builder.Services.AddSingleton<IEnglandPerformanceService, EnglandPerformanceService>();

        builder.Services.AddSingleton<IGenericRepository<EnglandDestinations>, JSONRepository<EnglandDestinations>>();
        builder.Services.AddSingleton<IEnglandDestinationsRepository, EnglandDestinationsRepository>();
        builder.Services.AddSingleton<IEnglandDestinationsService, EnglandDestinationsService>();




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions { SourceCodeLineCount = 1 });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Security headers middleware - MUST come before static files
        app.UseSecurityHeaders();

        app.UseHttpsRedirection();

        // Configure MIME types
        var provider = new FileExtensionContentTypeProvider();
        provider.Mappings[".css"] = "text/css";
        provider.Mappings[".js"] = "application/javascript";
        provider.Mappings[".mjs"] = "application/javascript";

        // Log wwwroot contents on startup for debugging
        var wwwrootPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
        if (Directory.Exists(wwwrootPath))
        {
            Console.WriteLine($"=== wwwroot exists at: {wwwrootPath} ===");
            var files = Directory.GetFiles(wwwrootPath, "*.*", SearchOption.AllDirectories)
                .Take(20)
                .Select(f => f.Replace(wwwrootPath, ""));
            Console.WriteLine("Sample files:");
            foreach (var file in files)
            {
                Console.WriteLine($"  {file}");
            }
        }
        else
        {
            Console.WriteLine($"WARNING: wwwroot directory not found at {wwwrootPath}");
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = provider,
            OnPrepareResponse = ctx =>
            {
                var path = ctx.Context.Request.Path.Value;
                var contentType = ctx.Context.Response.ContentType;
                Console.WriteLine($"Static file request: {path} -> {contentType ?? "NO CONTENT TYPE"}");

                if (string.IsNullOrEmpty(contentType))
                {
                    var ext = Path.GetExtension(path);
                    Console.WriteLine($"  WARNING: No content type for extension: {ext}");
                }
            }
        });

        app.UseRouting();
        app.UseGovUkFrontend();

        app.MapControllers();
        app.MapRazorPages();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Health check endpoints for AKS
        app.MapHealthChecks("/healthcheck");

        app.Run();
    }
}