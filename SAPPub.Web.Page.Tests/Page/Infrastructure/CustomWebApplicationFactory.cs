using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Performance;

namespace SAPPub.Web.Page.Tests.Tests.Infrastructure;

public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program>
     where Program : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureServices(services =>
            {
                // use mock services
                services.RemoveAll(typeof(IAboutSchoolService));
                services.AddSingleton<MockAccessor<IAboutSchoolService>>();
                services.AddSingleton<MockAccessor<IAttainmentAndProgressService>>();
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAboutSchoolService>>().Get()?.Object!;
                });
                services.AddTransient(provider =>
                {
                    return provider.GetRequiredService<MockAccessor<IAttainmentAndProgressService>>().Get()?.Object!;
                });
            });
    }
}

