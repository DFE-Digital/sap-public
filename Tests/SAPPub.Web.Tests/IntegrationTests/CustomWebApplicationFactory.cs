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

        builder.UseEnvironment(isCi ? "Testing" : "Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // ensure we read appsettings + appsettings.{ENV}.json + env vars
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                  .AddEnvironmentVariables();
        });

        builder.ConfigureServices((context, services) =>
        {
            if (isCi)
            {
                // CI: don't hit DB at all
                services.RemoveAll(typeof(IGenericRepository<>));
                services.AddTransient(typeof(IGenericRepository<>), typeof(FakeGenericRepository<>));

                // also ensure we don't accidentally create a real datasource
                services.RemoveAll<NpgsqlDataSource>();
                return;
            }

            // Local dev: use real local Postgres from appsettings.Development.json
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
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}
