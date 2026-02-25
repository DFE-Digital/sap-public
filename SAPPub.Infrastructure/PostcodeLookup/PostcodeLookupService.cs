using SAPPub.Core.Entities.PostcodeLookup;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Infrastructure.PostcodeLookup;

public class PostcodeLookupService : IPostcodeLookupService
{
    private readonly HttpClient _httpClient;

    public PostcodeLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.postcodes.io/");
    }

    public async Task<PostcodeResponseModel?> GetLatitudeAndLongitudeAsync(string? postcode)
    {
        if (string.IsNullOrWhiteSpace(postcode))
        {
            return null;
        }

        var response = await _httpClient.GetAsync($"postcodes/{postcode}");

        var json = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<PostcodeResponseModel>(json);
        return result;
    }
}
