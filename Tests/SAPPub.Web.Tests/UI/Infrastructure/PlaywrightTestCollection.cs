using Xunit;

namespace SAPPub.Web.Tests.UI.Infrastructure
{
    [CollectionDefinition("Playwright Tests", DisableParallelization = true)]
    public class PlaywrightTestCollection : ICollectionFixture<WebApplicationSetupFixture>
    {
    }
}

