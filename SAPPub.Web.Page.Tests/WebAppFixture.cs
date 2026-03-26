using AngleSharp;
using AngleSharp.Dom;

namespace SAPPub.Web.Page.Tests;

public class WebAppFixture : IDisposable
{
    public HttpClient Client { get; }
    public CustomWebApplicationFactory<Program> Factory { get; }

    public WebAppFixture()
    {
        Factory = new CustomWebApplicationFactory<Program>();
        Client = Factory.CreateClient();
    }

    public async Task<IDocument> BrowseToPage(string url)
    {
        var response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();

        // Anglesharp bit
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));
        return document;
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
