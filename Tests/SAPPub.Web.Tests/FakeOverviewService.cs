using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Core.ServiceModels.Overview;

namespace SAPPub.Web.Tests;

public sealed class FakeOverviewService : IOverviewService
{
    private static readonly Dictionary<string, OverviewModel> Overviews =
        new()
        {
            ["143034"] = CreateOverview(
                urn: "143034",
                schoolName: "St Paul's Church of England Academy",
                isKS2: true,
                isKS4: false,
                isKS5: false),

            ["135600"] = CreateOverview(
                urn: "135600",
                schoolName: "Ark Academy",
                isKS2: true,
                isKS4: true,
                isKS5: true),

            ["150009"] = CreateOverview(
                urn: "150009",
                schoolName: "Abraham Moss Community School",
                isKS2: true,
                isKS4: true,
                isKS5: false),

            ["137552"] = CreateOverview(
                urn: "137552",
                schoolName: "Stewards Academy - Science Specialist, Harlow",
                isKS2: false,
                isKS4: true,
                isKS5: false),

            ["149328"] = CreateOverview(
                urn: "149328",
                schoolName: "King Edward VI High School",
                isKS2: false,
                isKS4: true,
                isKS5: true),

            ["130499"] = CreateOverview(
                urn: "130499",
                schoolName: "Holy Cross College",
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

    private static OverviewModel CreateOverview(
        string urn,
        string schoolName,
        bool isKS2,
        bool isKS4,
        bool isKS5)
    {
        return new OverviewModel
        {
            Urn = urn,
            SchoolName = schoolName,

            PhaseOfEducation = GetPhase(
                isKS2,
                isKS4,
                isKS5),

            AgeRangeLow = string.Empty,
            AgeRangeHigh = string.Empty,
            NumberOfPupils = string.Empty,
            SenProvision = null,
            Phone = string.Empty,
            Website = string.Empty,

            // Valid enough for the map/view-model path.
            Easting = "430000",
            Northing = "380000",

            IsKS2 = isKS2,
            IsKS4 = isKS4,
            IsKS5 = isKS5,

            Attainment8 = null,
            EnglishAndMathsGrade5Establishment = null,
            EnglishAndMathsGrade5LA = null,
            EnglishAndMathsGrade5England = null,
            MoreThanOneForeignLanguage = null,

            DestinationsEstablishment = null,
            DestinationsLA = null,
            DestinationsEngland = null,

            ReadingWritingMathsExpectedEstablishment = null,
            ReadingWritingMathsExpectedLA = null,
            ReadingWritingMathsExpectedEngland = null,

            ReadingWritingMathsHigherEstablishment = null,
            ReadingWritingMathsHigherLA = null,
            ReadingWritingMathsHigherEngland = null
        };
    }

    private static string GetPhase(
        bool isKS2,
        bool isKS4,
        bool isKS5)
    {
        if (isKS2 && isKS4 && isKS5)
            return "All-through";

        if (isKS2 && isKS4)
            return "All-through";

        if (isKS4 && isKS5)
            return "Secondary";

        if (isKS2)
            return "Primary";

        if (isKS4)
            return "Secondary";

        if (isKS5)
            return "16 to 19";

        return string.Empty;
    }
}