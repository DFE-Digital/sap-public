using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Infrastructure.Repositories.Generic;          // FakeGenericRepository<>
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Web.Tests.IntegrationTests
{
    public class DependencyInjectionSmokeTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public DependencyInjectionSmokeTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        private static bool IsCi =>
            string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), "true", StringComparison.OrdinalIgnoreCase);

        [Fact]
        public void ServiceProvider_Resolves_Critical_Services()
        {
            using var scope = _factory.Services.CreateScope();
            var sp = scope.ServiceProvider;

            // Critical plumbing
            _ = sp.GetRequiredService<NpgsqlDataSource>();

            // Closed generic - proves generic repo wiring is valid
            _ = sp.GetRequiredService<IGenericRepository<Establishment>>();

            // Recent refactor area
            _ = sp.GetRequiredService<IEstablishmentSubjectEntriesRepository>();
        }

        [Fact]
        public void NpgsqlDataSource_IsSingleton()
        {
            using var scope1 = _factory.Services.CreateScope();
            using var scope2 = _factory.Services.CreateScope();

            var ds1 = scope1.ServiceProvider.GetRequiredService<NpgsqlDataSource>();
            var ds2 = scope2.ServiceProvider.GetRequiredService<NpgsqlDataSource>();

            Assert.Same(ds1, ds2);
        }

        [Fact]
        public async Task App_Boots_And_Health_Endpoint_Responds()
        {
            var client = _factory.CreateClient();

            // Your Program.cs maps: app.MapHealthChecks("/healthcheck");
            var response = await client.GetAsync("/healthcheck");

            Assert.True(
                response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent,
                $"Expected 200/204 from /healthcheck but got {(int)response.StatusCode} {response.StatusCode}");
        }

        [Fact]
        public void Testing_Config_Has_PostgresConnectionString()
        {
            using var scope = _factory.Services.CreateScope();
            var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var cs = cfg.GetConnectionString("PostgresConnectionString");
            Assert.False(string.IsNullOrWhiteSpace(cs));
        }

        [Fact]
        public void GenericRepository_IsTransient()
        {
            using var scope = _factory.Services.CreateScope();
            var sp = scope.ServiceProvider;

            var r1 = sp.GetRequiredService<IGenericRepository<Establishment>>();
            var r2 = sp.GetRequiredService<IGenericRepository<Establishment>>();

            Assert.NotSame(r1, r2);
        }

        [Fact]
        public void GenericRepository_Uses_Fake_In_CI_And_NotFake_Locally()
        {
            using var scope = _factory.Services.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Establishment>>();

            if (IsCi)
                Assert.IsType<FakeGenericRepository<Establishment>>(repo);
            else
                Assert.IsNotType<FakeGenericRepository<Establishment>>(repo);
        }
    }
}
