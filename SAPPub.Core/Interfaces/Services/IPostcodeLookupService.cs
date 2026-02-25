using SAPPub.Core.Entities.PostcodeLookup;

namespace SAPPub.Core.Interfaces.Services;

public interface IPostcodeLookupService
{
    public Task<PostcodeResponseModel?> GetLatitudeAndLongitudeAsync(string? postcode);
}
