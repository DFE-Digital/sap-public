using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.SchoolSearch;

[ExcludeFromCodeCoverage]
public record SchoolSearchResult
 (
     string Name,
     string URN,
     string EstablishmentName,
     string AddressStreet,
     string AddressLocality,
     string AddressAddress3,
     string AddressTown,
     string AddressPostcode,
     string LAName
 )
{
    public static SchoolSearchResult FromNameAndEstablishment(string name, Establishment establishment) => new(
        name,
        establishment.URN,
        establishment.EstablishmentName,
        establishment.AddressStreet,
        establishment.AddressLocality,
        establishment.AddressAddress3,
        establishment.AddressTown,
        establishment.AddressPostcode,
        establishment.LAName);
}
