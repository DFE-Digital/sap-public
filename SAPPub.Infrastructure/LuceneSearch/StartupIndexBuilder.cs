using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Infrastructure.LuceneSearch;

/// <summary>
/// creates the Lucene search index for schools at application startup.
/// </summary>
/// <param name="logger"></param>
/// <param name="writer"></param>
/// <param name="establishmentService"></param>
public class StartupIndexBuilder(ILogger<StartupIndexBuilder> logger, LuceneSchoolSearchIndexWriter writer, IEstablishmentService establishmentService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartupIndexBuilder starting...");

        // Wait until DB is reachable by retrying a minimal call.
        await WaitForDatabaseAsync(cancellationToken);

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


    /// <summary>
    /// Waits indefinitely until the database exists AND is reachable.
    /// The only conditions that break this loop:
    ///  - DB becomes reachable -> return immediately
    ///  - App receives shutdown signal -> OperationCanceledException thrown
    /// </summary>
    private async Task WaitForDatabaseAsync(CancellationToken ct)
    {

        var delay = TimeSpan.FromSeconds(3);
        var maxDelay = TimeSpan.FromSeconds(20);
        var attempt = 0;

        logger.LogInformation("Waiting for database to become available and populated (infinite wait mode)...");

        while (true)
        {
            ct.ThrowIfCancellationRequested();
            attempt++;

            try
            {
                // Minimal query — does the DB exist yet?
                var result = await establishmentService.GetEstablishmentsAsync(page: 1, take: 1, ct);
                if (result.Any())
                {
                    logger.LogInformation("Database is reachable after {Attempt} attempt(s).", attempt);
                    return;
                }
                else
                {
                    logger.LogWarning("Request to v_Establishment returned 0 results");
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }

            catch (Exception ex)
            {
                logger.LogWarning(
                    ex,
                    "Database not ready on attempt {Attempt}. Retrying in {Delay}…",
                    attempt,
                    delay
                );
            }

            await Task.Delay(delay, ct);

            // simple exponential backoff
            double next = Math.Min(delay.TotalSeconds * 2, maxDelay.TotalSeconds);
            delay = TimeSpan.FromSeconds(next);

        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
