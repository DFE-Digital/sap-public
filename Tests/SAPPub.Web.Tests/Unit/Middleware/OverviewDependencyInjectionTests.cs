using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SAPPub.Core.Interfaces.Repositories.Overview;
using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Core.Services.Overview;
using SAPPub.Infrastructure.Repositories.Overview;
using SAPPub.Web.Middleware;

namespace SAPPub.Web.Tests.Unit.Middleware;

public class OverviewDependencyInjectionTests
{
    [Fact]
    public void AddDependencies_RegistersOverviewRepositoryAndServiceAsTransient()
    {
        var services = new ServiceCollection();
        var environment = new Mock<IHostEnvironment>();
        var configuration = new ConfigurationBuilder().Build();

        services.AddDependencies(environment.Object, configuration);

        var repositoryRegistration = Assert.Single(
            services.Where(x => x.ServiceType == typeof(IOverviewRepository)));
        Assert.Equal(typeof(OverviewRepository), repositoryRegistration.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, repositoryRegistration.Lifetime);

        var serviceRegistration = Assert.Single(
            services.Where(x => x.ServiceType == typeof(IOverviewService)));
        Assert.Equal(typeof(OverviewService), serviceRegistration.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, serviceRegistration.Lifetime);
    }
}
