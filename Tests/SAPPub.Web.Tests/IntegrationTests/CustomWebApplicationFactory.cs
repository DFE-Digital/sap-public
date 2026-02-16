using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Data;

namespace SAPPub.Web.Tests.IntegrationTests;
// Custom factory for better control over the test environment
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Provide a value so Program.Main doesn't throw.
            // It won't actually connect unless you open/use it.
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:PostgresConnectionString"] =
                    "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"
            };

            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            // Optional but useful: ensure IDbConnection uses the same dummy string.
            services.RemoveAll<NpgsqlDataSource>();

            services.AddSingleton<NpgsqlDataSource>(_ =>
                NpgsqlDataSource.Create("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres"));
        });
    }
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Ensure the content root is set correctly for tests
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        return base.CreateHost(builder);
    }
}