using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Infrastructure.Repositories;

namespace SAPPub.Infrastructure.LuceneSearch;

public class StartupIndexBuilder(ILogger<StartupIndexBuilder> logger, LuceneIndexWriter writer, IEstablishmentService establishmentService, DBHealthCheck dBHealthCheckService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("reading Establishment Data From CSV at startup...");

        while (await dBHealthCheckService.IsDatabaseAvailableAsync() == false)
        {
            logger.LogWarning("Database is not available yet. Retrying in 60 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
        }

        var schools = await establishmentService.GetAllEstablishmentsAsync(cancellationToken);

        logger.LogInformation($"{schools.Count()} Establishments retrieved successfully");

        logger.LogInformation("Building Lucene index at startup...");

        writer.BuildIndex(schools);

        logger.LogInformation("Lucene index built successfully");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
