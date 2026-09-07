namespace SAPPub.Web.Tests.UI.Infrastructure;

[CollectionDefinition(
    "Playwright Overview Disabled Tests",
    DisableParallelization = true)]
public class OverviewDisabledPlaywrightTestCollection
    : ICollectionFixture<
        OverviewDisabledWebApplicationSetupFixture>
{
}