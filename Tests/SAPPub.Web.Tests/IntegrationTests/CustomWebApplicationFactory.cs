using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var isCi = string.Equals(
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS"),
            "true",
            StringComparison.OrdinalIgnoreCase);

        // CI: run in UITests so Program doesn't insist on a real connection string.
        // Local: Development so appsettings.Development.json is used.
        builder.UseEnvironment(isCi ? "UITests" : "Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Ensure we read appsettings + appsettings.{ENV}.json + env vars
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                  .AddEnvironmentVariables();
        });

        builder.ConfigureServices((context, services) =>
        {
            if (isCi)
            {
                // -------------------------
                // CI: no DB + no /keys access
                // -------------------------

                // Swap out the generic repo so nothing uses Dapper/Npgsql.
                services.RemoveAll(typeof(IGenericRepository<>));
                services.AddTransient(typeof(IGenericRepository<>), typeof(FakeGenericRepository<>));

                // Ensure we don't accidentally resolve/use a real datasource
                services.RemoveAll<NpgsqlDataSource>();

                // Avoid DataProtection trying to read/write /keys in CI
                // (Program registers PersistKeysToFileSystem("/keys"))
                services.RemoveAll<IConfigureOptions<KeyManagementOptions>>();
                services.RemoveAll<IConfigureOptions<DataProtectionOptions>>();

                services.AddDataProtection()
                        .UseEphemeralDataProtectionProvider()
                        .SetApplicationName("SAPPub.Tests");

                return;
            }

            // -------------------------
            // Local dev: use real local Postgres from appsettings.Development.json
            // -------------------------
            services.RemoveAll<NpgsqlDataSource>();

            services.AddSingleton(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var cs = cfg.GetConnectionString("PostgresConnectionString");

                if (string.IsNullOrWhiteSpace(cs))
                    throw new InvalidOperationException(
                        "Expected ConnectionStrings:PostgresConnectionString in appsettings.Development.json for local test runs.");

                return NpgsqlDataSource.Create(cs);
            });

            // Optional: you *can* also keep ephemeral DP keys locally for consistency,
            // but it's not required if /keys is writable on your machine.
            // If you want it, uncomment:
            /*
            services.RemoveAll<IConfigureOptions<KeyManagementOptions>>();
            services.RemoveAll<IConfigureOptions<DataProtectionOptions>>();
            services.AddDataProtection()
                    .UseEphemeralDataProtectionProvider()
                    .SetApplicationName("SAPPub.Tests");
            */
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}
