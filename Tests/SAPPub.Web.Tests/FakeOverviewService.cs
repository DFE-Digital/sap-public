using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Core.ServiceModels.Overview;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Tests;

public sealed class FakeOverviewService : IOverviewService
{
    public const string CompleteOverviewUrn = "143034";

    public const string MissingDataOverviewUrn = "137552";

    private static readonly Dictionary<string, OverviewModel> Overviews =
        new()
        {
            ["143034"] = CreateCompleteOverview(),

            ["135600"] = CreateOverview(
                urn: "135600",
                schoolName: "Ark Academy",
                localAuthorityName: "Brent",
                isKS2: true,
                isKS4: true,
                isKS5: true),

            ["150009"] = CreateOverview(
                urn: "150009",
                schoolName: "Abraham Moss Community School",
                localAuthorityName: "Manchester",
                isKS2: true,
                isKS4: true,
                isKS5: false),

            ["137552"] = CreateMissingDataOverview(),

            ["149328"] = CreateOverview(
                urn: "149328",
                schoolName: "King Edward VI High School",
                localAuthorityName: "Sheffield",
                isKS2: false,
                isKS4: true,
                isKS5: true),

            ["130499"] = CreateOverview(
                urn: "130499",
                schoolName: "Holy Cross College",
                localAuthorityName: "Bury",
                isKS2: false,
                isKS4: false,
                isKS5: true)
        };

    public Task<OverviewModel?> GetOverviewAsync(
        string urn,
        CancellationToken ct = default)
    {
        Overviews.TryGetValue(
            urn,
            out var overview);

        return Task.FromResult(overview);
    }

    private static OverviewModel CreateCompleteOverview()
    {
        return new OverviewModel
        {
            Urn = CompleteOverviewUrn,

            SchoolName =
                "St Paul's Church of England Academy",

            LocalAuthorityName = "Birmingham",

            PhaseOfEducation = "Primary",

            AgeRangeLow = "2",
            AgeRangeHigh = "11",

            NumberOfPupils = "661",

            SenProvision =
                "ASD - Autistic Spectrum Disorder",

            Phone = "01424 424530",

            Website =
                "www.stpaulsceacademy.org",

            Address =
                "Grove Lane, Handsworth, Birmingham, B21 9ET",

            // Valid OSGB coordinates so the map initialises.
            Easting = "405900",
            Northing = "289500",

            IsKS2 = true,
            IsKS4 = false,
            IsKS5 = false
        };
    }

    private static OverviewModel CreateMissingDataOverview()
    {
        return new OverviewModel
        {
            Urn = MissingDataOverviewUrn,

            SchoolName =
                "Stewards Academy - Science Specialist, Harlow",

            LocalAuthorityName = "Essex",

            PhaseOfEducation = "Secondary",

            AgeRangeLow = "",
            AgeRangeHigh = "",

            NumberOfPupils = "",

            SenProvision = null,

            Phone = "",

            Website = "",

            Address = "",

            // Leave location empty for the missing-location scenario too.
            Easting = "",
            Northing = "",

            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false,

            Attainment8 = null,
            Attainment8LA = null,
            Attainment8England = null
        };
    }

    private static OverviewModel CreateOverview(
        string urn,
        string schoolName,
        string localAuthorityName,
        bool isKS2,
        bool isKS4,
        bool isKS5)
    {
        return new OverviewModel
        {
            Urn = urn,

            SchoolName = schoolName,

            LocalAuthorityName =
                localAuthorityName,

            PhaseOfEducation = GetPhase(
                isKS2,
                isKS4,
                isKS5),

            AgeRangeLow = "11",
            AgeRangeHigh = "18",

            NumberOfPupils = "1000",

            SenProvision =
                "ASD - Autistic Spectrum Disorder",

            Phone = "0121 555 1234",

            Website = "www.example.com",

            Address =
                "1 Test Street, Test Town, TE1 1ST",

            Easting = "405900",
            Northing = "289500",

            IsKS2 = isKS2,
            IsKS4 = isKS4,
            IsKS5 = isKS5,

            Attainment8 = new CodedDouble(
                49.9,
                string.Empty,
                "49.9"),

            Attainment8LA = new CodedDouble(
                43.4,
                string.Empty,
                "43.4"),

            Attainment8England = new CodedDouble(
                45.2,
                string.Empty,
                "45.2")
        };
    }

    private static string GetPhase(
        bool isKS2,
        bool isKS4,
        bool isKS5)
    {
        if (isKS2 && isKS4 && isKS5)
        {
            return "All-through";
        }

        if (isKS2 && isKS4)
        {
            return "All-through";
        }

        if (isKS4 && isKS5)
        {
            return "Secondary";
        }

        if (isKS2)
        {
            return "Primary";
        }

        if (isKS4)
        {
            return "Secondary";
        }

        if (isKS5)
        {
            return "16 to 19";
        }

        return string.Empty;
    }
}