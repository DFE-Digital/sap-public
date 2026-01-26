using SAPPub.Core.Entities;

namespace SAPPub.Core.Tests.TestBuilders;

public class EstablishmentTestBuilder
{
    private readonly Establishment _establishment = new();

    public static string GenerateUrn()
    {
        // Generates a random 6-digit URN as a string
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public static string GenerateEstablishmentName()
    {
        // Generates a random establishment name
        var adjectives = new[] { "Green", "Oak", "River", "Hill", "Sunny", "Maple", "Elm", "Cedar" };
        var types = new[] { "Primary", "Secondary", "Academy", "School", "College" };
        var suffixes = new[] { "Academy", "School", "College", "Institute" };

        var random = new Random();
        var adjective = adjectives[random.Next(adjectives.Length)];
        var type = types[random.Next(types.Length)];
        var suffix = suffixes[random.Next(suffixes.Length)];

        return $"{adjective} {type} {suffix}";
    }

    public EstablishmentTestBuilder WithURN(string urn)
    {
        _establishment.URN = urn;
        return this;
    }

    public EstablishmentTestBuilder WithEstablishmentName(string name)
    {
        _establishment.EstablishmentName = name;
        return this;
    }

    public EstablishmentTestBuilder WithTrustsId(string trustsId)
    {
        _establishment.TrustsId = trustsId;
        return this;
    }

    public EstablishmentTestBuilder WithTrustName(string trustName)
    {
        _establishment.TrustName = trustName;
        return this;
    }

    public EstablishmentTestBuilder WithAddressStreet(string street)
    {
        _establishment.AddressStreet = street;
        return this;
    }

    public EstablishmentTestBuilder WithAddressLocality(string locality)
    {
        _establishment.AddressLocality = locality;
        return this;
    }

    public EstablishmentTestBuilder WithAddressAddress3(string address3)
    {
        _establishment.AddressAddress3 = address3;
        return this;
    }

    public EstablishmentTestBuilder WithAddressTown(string town)
    {
        _establishment.AddressTown = town;
        return this;
    }

    public EstablishmentTestBuilder WithAddressPostcode(string postcode)
    {
        _establishment.AddressPostcode = postcode;
        return this;
    }

    public EstablishmentTestBuilder WithAdmissionsPolicyId(string admissionsPolicyId)
    {
        _establishment.AdmissionsPolicyId = admissionsPolicyId;
        return this;
    }

    public EstablishmentTestBuilder WithAdmissionPolicy(string admissionPolicy)
    {
        _establishment.AdmissionPolicy = admissionPolicy;
        return this;
    }

    public EstablishmentTestBuilder WithAgeRangeLow(string ageRangeLow)
    {
        _establishment.AgeRangeLow = ageRangeLow;
        return this;
    }

    public EstablishmentTestBuilder WithAgeRangeHigh(string ageRangeHigh)
    {
        _establishment.AgeRangeHigh = ageRangeHigh;
        return this;
    }

    public EstablishmentTestBuilder WithDFENumber(string dfeNumber)
    {
        _establishment.DFENumber = dfeNumber;
        return this;
    }

    public EstablishmentTestBuilder WithDistrictAdministrativeId(string id)
    {
        _establishment.DistrictAdministrativeId = id;
        return this;
    }

    public EstablishmentTestBuilder WithDistrictAdministrativeName(string name)
    {
        _establishment.DistrictAdministrativeName = name;
        return this;
    }

    public EstablishmentTestBuilder WithPhaseOfEducationId(string id)
    {
        _establishment.PhaseOfEducationId = id;
        return this;
    }

    public EstablishmentTestBuilder WithPhaseOfEducationName(string name)
    {
        _establishment.PhaseOfEducationName = name;
        return this;
    }

    public EstablishmentTestBuilder WithGenderId(string id)
    {
        _establishment.GenderId = id;
        return this;
    }

    public EstablishmentTestBuilder WithGenderName(string name)
    {
        _establishment.GenderName = name;
        return this;
    }

    public EstablishmentTestBuilder WithHeadteacherTitle(string title)
    {
        _establishment.HeadteacherTitle = title;
        return this;
    }

    public EstablishmentTestBuilder WithHeadteacherFirstName(string firstName)
    {
        _establishment.HeadteacherFirstName = firstName;
        return this;
    }

    public EstablishmentTestBuilder WithHeadteacherLastName(string lastName)
    {
        _establishment.HeadteacherLastName = lastName;
        return this;
    }

    public EstablishmentTestBuilder WithHeadteacherPreferredJobTitle(string jobTitle)
    {
        _establishment.HeadteacherPreferredJobTitle = jobTitle;
        return this;
    }

    public EstablishmentTestBuilder WithOfficialSixthFormId(string id)
    {
        _establishment.OfficialSixthFormId = id;
        return this;
    }

    public EstablishmentTestBuilder WithLAId(string laId)
    {
        _establishment.LAId = laId;
        return this;
    }

    public EstablishmentTestBuilder WithLAName(string laName)
    {
        _establishment.LAName = laName;
        return this;
    }

    public EstablishmentTestBuilder WithReligiousCharacterId(string id)
    {
        _establishment.ReligiousCharacterId = id;
        return this;
    }

    public EstablishmentTestBuilder WithReligiousCharacterName(string name)
    {
        _establishment.ReligiousCharacterName = name;
        return this;
    }

    public EstablishmentTestBuilder WithTelephoneNum(string telephoneNum)
    {
        _establishment.TelephoneNum = telephoneNum;
        return this;
    }

    public EstablishmentTestBuilder WithTotalPupils(string totalPupils)
    {
        _establishment.TotalPupils = totalPupils;
        return this;
    }

    public EstablishmentTestBuilder WithTypeOfEstablishmentId(string id)
    {
        _establishment.TypeOfEstablishmentId = id;
        return this;
    }

    public EstablishmentTestBuilder WithTypeOfEstablishmentName(string name)
    {
        _establishment.TypeOfEstablishmentName = name;
        return this;
    }

    public EstablishmentTestBuilder WithResourcedProvision(string resourcedProvision)
    {
        _establishment.ResourcedProvision = resourcedProvision;
        return this;
    }

    public EstablishmentTestBuilder WithUKPRN(string ukprn)
    {
        _establishment.UKPRN = ukprn;
        return this;
    }

    public EstablishmentTestBuilder WithUrbanRuralId(string id)
    {
        _establishment.UrbanRuralId = id;
        return this;
    }

    public EstablishmentTestBuilder WithUrbanRuralName(string name)
    {
        _establishment.UrbanRuralName = name;
        return this;
    }

    public EstablishmentTestBuilder WithWebsite(string website)
    {
        _establishment.Website = website;
        return this;
    }

    public EstablishmentTestBuilder WithEasting(string easting)
    {
        _establishment.Easting = easting;
        return this;
    }

    public EstablishmentTestBuilder WithNorthing(string northing)
    {
        _establishment.Northing = northing;
        return this;
    }

    public EstablishmentTestBuilder WithEstablishmentNumber(string establishmentNumber)
    {
        _establishment.EstablishmentNumber = establishmentNumber;
        return this;
    }

    public Establishment Build()
    {
        // fill basic values automatically if not set
        if (string.IsNullOrEmpty(_establishment.URN))
        {
            _establishment.URN = GenerateUrn();
        }
        if (string.IsNullOrEmpty(_establishment.EstablishmentName))
        {
            _establishment.EstablishmentName = GenerateEstablishmentName();
        }
        return _establishment;
    }
}