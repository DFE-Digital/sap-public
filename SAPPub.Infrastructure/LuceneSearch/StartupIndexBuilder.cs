using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Infrastructure.LuceneSearch;

public class StartupIndexBuilder(ILogger<StartupIndexBuilder> logger, LuceneIndexWriter writer, IEstablishmentService establishmentService) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("reading Establishment Data From CSV at startup...");

        var schools = establishmentService.GetAllEstablishments();

        logger.LogInformation("Establishment Data retrieved successfully");

        // CML TODO - the index won't be recalculated when data is updated from the pipeline
        // Co-pilot-generated message - the index won't be built if the app is running in a container and the index is stored on a volume,
        // as the index files will be locked by the first instance that builds the index. We should consider building
        // the index as part of the image build process instead, or using a different approach to building the index at
        // runtime that doesn't involve locking files.
        logger.LogInformation("Building Lucene index at startup...");

        writer.BuildIndex(schools);

        logger.LogInformation("Lucene index built successfully");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
