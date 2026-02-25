namespace SAPPub.Core.Entities.PostcodeLookup;

public record PostcodeResponseModel
{
    public int Status { get; set; }
    public PostcodeResultModel? result { get; init; }
}

public record PostcodeResultModel
{
    public string? postcode { get; init; }
    public float latitude { get; init; }
    public float longitude { get; init; }
}
