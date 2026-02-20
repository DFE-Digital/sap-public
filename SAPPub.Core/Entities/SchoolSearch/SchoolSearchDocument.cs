using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.SchoolSearch;

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
