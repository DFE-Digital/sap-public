using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace SAPPub.Web.Tests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Build whatever config sources already exist (env vars, appsettings, etc.)
            var built = config.Build();
            var existing = built.GetConnectionString("PostgresConnectionString");

            // If CI has provided a connection string, don't override it.
            if (!string.IsNullOrWhiteSpace(existing))
                return;

            // Local fallback only (so tests still run locally without setup)
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgresConnectionString"] =
                    "Host=127.0.0.1;Port=1;Database=x;Username=x;Password=x;Timeout=1;Command Timeout=1"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Ensure NpgsqlDataSource uses the configured connection string
            services.RemoveAll<NpgsqlDataSource>();

            services.AddSingleton(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var cs = cfg.GetConnectionString("PostgresConnectionString")
                         ?? throw new InvalidOperationException("Connection string 'PostgresConnectionString' is not configured.");

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
