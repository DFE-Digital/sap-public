namespace SAPPub.Core.ServiceModels.PostcodeLookup;

public record PostcodeResponseModel
{
    public int Status { get; init; }
    public string? Error { get; init; }
    public PostcodeResultModel? Result { get; init; }
}

public record PostcodeResultModel
{
    public string? Postcode { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}
