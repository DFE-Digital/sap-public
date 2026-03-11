using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Infrastructure.LuceneSearch;

public class StartupIndexBuilder(ILogger<StartupIndexBuilder> logger, LuceneIndexWriter writer, IEstablishmentService establishmentService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("reading Establishment Data From CSV at startup...");

        int page = 1;
        int take = 1000;
        var schools = await establishmentService.GetEstablishmentsAsync(page, take, cancellationToken);
        while (schools.Any())
        {
            writer.AddToIndex(schools);
            page++;
            schools = await establishmentService.GetEstablishmentsAsync(page, take, cancellationToken);
        }

        logger.LogInformation("Establishment Data retrieved successfully");

        logger.LogInformation("Building Lucene index at startup...");


        writer.FinaliseIndex();

        logger.LogInformation("Lucene index built successfully");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
