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

        int page = 1;
        int take = 1000;

        // Wait until DB is reachable by retrying a minimal call.
        await WaitForDatabaseAsync(cancellationToken);

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

    private async Task WaitForDatabaseAsync(CancellationToken ct)
    {
        // Adjust these to taste
        var maxWait = TimeSpan.FromMinutes(3);     // or TimeSpan.FromHours(1) / infinite in review env
        var delay = TimeSpan.FromSeconds(1);
        var maxDelay = TimeSpan.FromSeconds(15);
        var started = DateTimeOffset.UtcNow;

        var attempt = 0;

        while (true)
        {
            ct.ThrowIfCancellationRequested();
            attempt++;

            try
            {
                // Minimal call: page=1,take=1 to avoid loading much.
                var result = await establishmentService.GetEstablishmentsAsync(page: 1, take: 1, ct);
                if (result.Count() == 1)
                {
                    return;
                }
                else
                {
                    logger.LogWarning("Request to v_Establishment returned 0 results");
                    await Task.Delay(TimeSpan.FromSeconds(5), ct);
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                var elapsed = DateTimeOffset.UtcNow - started;
                if (elapsed >= maxWait)
                {
                    logger.LogError(ex, "Database not available after waiting {Elapsed}. Giving up.", elapsed);
                    throw;
                }

                // Exponential backoff + jitter
                var jitterMs = Random.Shared.Next(0, 250);
                var sleep = delay + TimeSpan.FromMilliseconds(jitterMs);

                logger.LogWarning(ex,
                    "Database not ready yet. Attempt {Attempt}. Waiting {Delay} before next attempt...",
                    attempt, sleep);

                await Task.Delay(sleep, ct);

                var nextSeconds = Math.Min(delay.TotalSeconds * 2, maxDelay.TotalSeconds);
                delay = TimeSpan.FromSeconds(nextSeconds);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
