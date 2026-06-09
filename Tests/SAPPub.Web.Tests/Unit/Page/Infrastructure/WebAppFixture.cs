using AngleSharp;
using AngleSharp.Dom;
using SAPPub.Web.Tests.Unit.Page.Helpers;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public class WebAppFixture : IDisposable
{
    public HttpClient Client { get; }
    public CustomWebApplicationFactory<Program> Factory { get; }

    public WebAppFixture()
    {
        Factory = new CustomWebApplicationFactory<Program>();
        Client = Factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }

    public async Task<IDocument> BrowseToPage(string url)
    {
        var response = await Client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Status: {(int)response.StatusCode}\n{body}");
        }

        var html = await response.Content.ReadAsStringAsync();

        // Anglesharp bit
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));
        return document;
    }

    public async Task<PageResponse> PostToPage<T>(string url, T model)
    {
        var values = new List<KeyValuePair<string, string>>();

        foreach (var property in typeof(T).GetProperties())
        {
            var value = property.GetValue(model);

            if (value == null)
                continue;

            // Handle List<string>
            if (value is IEnumerable<string> list && value is not string)
            {
                foreach (var item in list)
                {
                    values.Add(new KeyValuePair<string, string>(
                        property.Name,
                        item));
                }
            }
            else
            {
                values.Add(new KeyValuePair<string, string>(
                    property.Name,
                    value.ToString()));
            }
        }

        var content = new FormUrlEncodedContent(values);

        var response = await Client.PostAsync(url, content);

        var html = await response.Content.ReadAsStringAsync();


        IDocument? document = null;

        if (!string.IsNullOrWhiteSpace(html))
        {
            var context = BrowsingContext.New(Configuration.Default);

            document = await context.OpenAsync(req => req.Content(html));
        }

        return new PageResponse
        {
            Document = document,
            StatusCode = response.StatusCode,
            RedirectionLocation = response.Headers.Location?.ToString()
        };
    }


    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
