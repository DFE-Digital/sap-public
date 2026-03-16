using SAPPub.Core.ServiceModels.PostcodeSearch;

namespace SAPPub.Core.Interfaces.Services;

public interface IPostcodeLookupService
{
    public Task<PostcodeResponseModel?> GetLatitudeAndLongitudeAsync(string postcode);
}
