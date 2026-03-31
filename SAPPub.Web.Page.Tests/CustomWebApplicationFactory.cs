using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Page.Tests;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>
     where Program : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            //.UseConfiguration(configuration)
            //.ConfigureAppConfiguration(configurationBuilder =>
            //{
            //    configurationBuilder.AddInMemoryCollection(configurationValues);
            //})
            .ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IGenericRepository<>));
                services.AddScoped(typeof(IGenericRepository<>), typeof(FakeGenericRepository<>)); // use FakeRepository as default

                // use mock repositories instead of FakeRepository here
                services.AddSingleton<MockAccessor<IGenericRepository<Establishment>>>();
                services.AddSingleton<MockAccessor<IGenericRepository<EstablishmentPerformance>>>();
                services.AddSingleton<MockAccessor<IGenericRepository<EnglandPerformance>>>();
                services.AddSingleton<MockAccessor<IGenericRepository<LAPerformance>>>();

                services.RemoveAll(typeof(IGenericRepository<Establishment>));
                services.AddTransient<IGenericRepository<Establishment>>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IGenericRepository<Establishment>>>();
                    return accessor.Get()?.Object ?? new FakeGenericRepository<Establishment>();
                });

                services.RemoveAll(typeof(IGenericRepository<EstablishmentPerformance>));
                services.AddTransient<IGenericRepository<EstablishmentPerformance>>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IGenericRepository<EstablishmentPerformance>>>();
                    return accessor.Get()?.Object ?? new FakeGenericRepository<EstablishmentPerformance>(); ;
                });

                services.RemoveAll(typeof(IGenericRepository<EnglandPerformance>));
                services.AddTransient<IGenericRepository<EnglandPerformance>>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IGenericRepository<EnglandPerformance>>>();
                    return accessor.Get()?.Object ?? new FakeGenericRepository<EnglandPerformance>(); ;
                });

                services.RemoveAll(typeof(IGenericRepository<LAPerformance>));
                services.AddTransient<IGenericRepository<LAPerformance>>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IGenericRepository<LAPerformance>>>();
                    return accessor.Get()?.Object ?? new FakeGenericRepository<LAPerformance>(); ;
                });
            });
    }
}

