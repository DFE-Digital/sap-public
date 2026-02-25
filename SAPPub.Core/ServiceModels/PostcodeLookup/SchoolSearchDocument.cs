using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.ServiceModels.PostcodeLookup;

[ExcludeFromCodeCoverage]
public record SchoolSearchResults(int Count, IList<SchoolSearchDocument> Results);

[ExcludeFromCodeCoverage]
public record SchoolSearchDocument(
     string? URN,
     string? EstablishmentName,
     string? Address,
     string? GenderName,
     string? ReligiousCharacterName
);
