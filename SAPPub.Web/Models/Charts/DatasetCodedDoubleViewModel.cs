using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Models.Charts;

public record DatasetCodedDoubleViewModel
{
    public string? Label { get; set; }

    public required List<CodedDouble> Data { get; init; }
}
