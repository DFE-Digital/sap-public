using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2PupilPerformance
{
    public required string Urn { get; init; }
    public CodedDouble EstablishmentReadingScore { get; init; }
    public CodedString EstablishmentReadingDescription { get; init; }
    public CodedDouble EstablishmentReadingConfidenceUpper { get; set; }
    public CodedDouble EstablishmentReadingConfidenceLower { get; set; }
    public CodedDouble LaReadingScore { get; init; }
    public CodedDouble EstablishmentWritingScore { get; init; }
    public CodedString EstablishmentWritingDescription { get; init; }
    public CodedDouble EstablishmentWritingConfidenceUpper { get; set; }
    public CodedDouble EstablishmentWritingConfidenceLower { get; set; }
    public CodedDouble LaWritingScore { get; init; }
    public CodedDouble EstablishmentMathsScore { get; init; }
    public CodedString EstablishmentMathsDescription { get; init; }
    public CodedDouble EstablishmentMathsConfidenceUpper { get; set; }
    public CodedDouble EstablishmentMathsConfidenceLower { get; set; }
    public CodedDouble LaMathsScore { get; init; }
}