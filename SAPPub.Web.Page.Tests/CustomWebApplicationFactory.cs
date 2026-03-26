using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAPPub.Core.Entities;
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
                //services.AddSingleton<MockAccessor<IGenericRepository<EstablishmentAbsence>>>();

                services.RemoveAll(typeof(IGenericRepository<Establishment>));
                services.AddTransient<IGenericRepository<Establishment>>(provider =>
                {
                    var accessor = provider.GetRequiredService<MockAccessor<IGenericRepository<Establishment>>>();
                    return accessor.GetOrDefault().Object;

                });
                //services.RemoveAll(typeof(IGenericRepository<EstablishmentAbsence>));
                //services.AddTransient<IGenericRepository<EstablishmentAbsence>>(provider =>
                //{
                //    var accessor = provider.GetRequiredService<MockAccessor<IGenericRepository<EstablishmentAbsence>>>();
                //    return accessor.Get().Object;
                //});
            });
    }
}

