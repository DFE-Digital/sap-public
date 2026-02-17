using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.SchoolSearch;

[ExcludeFromCodeCoverage]
public record SchoolSearchResults(int Count, IList<SchoolSearchResult> Results);

[ExcludeFromCodeCoverage]
public record SchoolSearchResult(
     string URN,
     string EstablishmentName,
     string Address,
     string GenderName,
     string ReligiousCharacterName
);
