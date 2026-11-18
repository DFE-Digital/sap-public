using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.EESCodeGenerator;
using SAPPub.EESCodeGenerator.Helpers;
using SAPPub.Infrastructure.EES.Helpers;
using SAPPub.Infrastructure.EES.Service;

internal class Program
{
    private async static Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
        });
        serviceCollection.AddHttpClient<IEESClient, EESClient>();
        serviceCollection.AddTransient<IEESAPIService, EESAPIService>();
        serviceCollection.AddTransient<IModelGeneratorWrapper, ModelGeneratorWrapper>();

        using var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        });

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogDebug("Starting application");

        var eesAPI = serviceProvider.GetRequiredService<IEESAPIService>();

        IModelGeneratorWrapper wrapper = serviceProvider.GetRequiredService<IModelGeneratorWrapper>();

        await wrapper.GenerateAllModels();

        logger.LogDebug("All done!");
    }
}