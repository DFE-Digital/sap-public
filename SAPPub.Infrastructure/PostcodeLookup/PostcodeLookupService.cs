using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels.PostcodeLookup;

namespace SAPPub.Infrastructure.PostcodeLookup;

public class PostcodeLookupService : IPostcodeLookupService
{
    private readonly HttpClient _httpClient;

    public PostcodeLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.postcodes.io/");
    }

    public async Task<PostcodeResponseModel?> GetLatitudeAndLongitudeAsync(string postcode)
    {
        if (string.IsNullOrWhiteSpace(postcode))
        {
            return null;
        }

        var response = await _httpClient.GetAsync($"postcodes/{postcode}");

        var json = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<PostcodeResponseModel>(
            json,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );
        return result;
    }
}
