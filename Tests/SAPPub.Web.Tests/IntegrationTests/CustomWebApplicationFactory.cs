using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
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

        // Use a dedicated env name for CI to avoid accidentally using real infra.
        builder.UseEnvironment(isCi ? "UITests" : "Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Ensure we read appsettings + appsettings.{ENV}.json + env vars
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                  .AddEnvironmentVariables();

            // In CI, guarantee a connection string exists (but it can be dummy).
            if (isCi)
            {
                // Port=1 makes it effectively unreachable if anything accidentally tries to connect.
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:PostgresConnectionString"] =
                        "Host=127.0.0.1;Port=1;Database=x;Username=x;Password=x;Timeout=1;Command Timeout=1"
                });
            }
        });

        builder.ConfigureServices((context, services) =>
        {
            //if (isCi)
            {
                // CI: don't hit DB at all - swap out generic repo
                services.RemoveAll(typeof(IGenericRepository<>));
                services.AddTransient(typeof(IGenericRepository<>), typeof(FakeGenericRepository<>));

                // BUT still register NpgsqlDataSource so DI smoke tests pass
                services.RemoveAll<NpgsqlDataSource>();
                services.AddSingleton(sp =>
                {
                    var cfg = sp.GetRequiredService<IConfiguration>();
                    var cs = cfg.GetConnectionString("PostgresConnectionString")
                             ?? throw new InvalidOperationException("Connection string 'PostgresConnectionString' is not configured.");

                    return NpgsqlDataSource.Create(cs);
                });

                return;
            }

            // Local dev laptop: use real local Postgres from appsettings.Development.json
            //services.RemoveAll<NpgsqlDataSource>();
            //services.AddSingleton(sp =>
            //{
            //    var cfg = sp.GetRequiredService<IConfiguration>();
            //    var cs = cfg.GetConnectionString("PostgresConnectionString");

            //    if (string.IsNullOrWhiteSpace(cs))
            //        throw new InvalidOperationException(
            //            "Expected ConnectionStrings:PostgresConnectionString in appsettings.Development.json for local test runs.");

            //    return NpgsqlDataSource.Create(cs);
            //});
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}
