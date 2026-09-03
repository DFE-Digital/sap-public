using SAPPub.Core.Extensions;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Overview;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Core.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Overview;

public sealed class OverviewViewModel : ProfileBaseViewModel
{
    public required DisplayField<string> EducationPhase { get; init; }

    public required DisplayField<string> AgeRange { get; init; }

    public required DisplayField<string> NumberOfPupils { get; init; }

    public required DisplayField<string> SenTypes { get; init; }

    public required DisplayField<string> Telephone { get; init; }

    public required DisplayField<string> SchoolWebsite { get; init; }

    public required DisplayField<string> Address { get; init; }

    public string Longitude { get; set; } = string.Empty;

    public string Latitude { get; set; } = string.Empty;

    public string OfstedReportUrl => $"http://reports.ofsted.gov.uk/inspection-reports/find-inspection-report/provider/ELS/{URN}";

    public required DisplayField<CodedDouble> Attainment8 { get; init; }

    public required DisplayField<CodedDouble> MoreThanOneForeignLanguage { get; init; }

    public required SimpleCodedDoubleTableViewModel? EnglishAndMathsGrade5 { get; init; }

    public required SimpleCodedDoubleTableViewModel? Destinations { get; init; }

    public required SimpleCodedDoubleTableViewModel? ReadingWritingMathsExpected { get; init; }

    public required SimpleCodedDoubleTableViewModel? ReadingWritingMathsHigher { get; init; }

    public static OverviewViewModel Map(OverviewModel model)
    {
        var latLong = MappingHelper.ConvertToLatLon(model.Easting, model.Northing);

        return new OverviewViewModel
        {
            URN = model.Urn,
            SchoolName = model.SchoolName,
            Address = model.Address.ToDisplayField(),

            Latitude = latLong?.Latitude.ToString() ?? string.Empty,
            Longitude = latLong?.Longitude.ToString() ?? string.Empty,

            IsKS2 = model.IsKS2,
            IsKS4 = model.IsKS4,
            IsKS5 = model.IsKS5,

            EducationPhase = EducationPhaseFormatter.Format(
                model.IsKS2,
                model.IsKS4,
                model.IsKS5)
                .ToDisplayField(),

            AgeRange = GetAgeRange(
                model.AgeRangeLow,
                model.AgeRangeHigh)
                .ToDisplayField(),

            NumberOfPupils = FormatNumberOfPupils(
                model.NumberOfPupils)
                .ToDisplayField(),

            SenTypes = model.SenProvision.ToDisplayField(),
            Telephone = model.Phone.ToDisplayField(),
            SchoolWebsite = model.Website.ToDisplayField(),
            Attainment8 = model.Attainment8.ToDisplayField(),
            MoreThanOneForeignLanguage = model.MoreThanOneForeignLanguage.ToDisplayField(),

            EnglishAndMathsGrade5 = MapComparison(
                model.EnglishAndMathsGrade5Establishment,
                model.EnglishAndMathsGrade5LA,
                model.EnglishAndMathsGrade5England),

            Destinations = MapComparison(
                model.DestinationsEstablishment,
                model.DestinationsLA,
                model.DestinationsEngland),

            ReadingWritingMathsExpected = MapComparison(
                model.ReadingWritingMathsExpectedEstablishment,
                model.ReadingWritingMathsExpectedLA,
                model.ReadingWritingMathsExpectedEngland),

            ReadingWritingMathsHigher = MapComparison(
                model.ReadingWritingMathsHigherEstablishment,
                model.ReadingWritingMathsHigherLA,
                model.ReadingWritingMathsHigherEngland)
        };
    }

    private static string? GetAgeRange(
        string? ageRangeLow,
        string? ageRangeHigh)
    {
        if (string.IsNullOrWhiteSpace(ageRangeLow) ||
            string.IsNullOrWhiteSpace(ageRangeHigh))
        {
            return null;
        }

        return $"{ageRangeLow} to {ageRangeHigh}";
    }

    private static string? FormatNumberOfPupils(string? numberOfPupils)
    {
        if (string.IsNullOrWhiteSpace(numberOfPupils))
        {
            return null;
        }

        return int.TryParse(numberOfPupils, out var pupils)
            ? pupils.ToString("N0")
            : numberOfPupils;
    }

    private static SimpleCodedDoubleTableViewModel? MapComparison(
    CodedDouble? schoolOrCollege,
    CodedDouble? localAuthority,
    CodedDouble? england)
    {
        if (!schoolOrCollege.HasValue ||
            !localAuthority.HasValue ||
            !england.HasValue)
        {
            return null;
        }

        return SimpleCodedDoubleTableViewModel.Map(
            new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = schoolOrCollege.Value,
                LocalAuthority = localAuthority.Value,
                England = england.Value
            });
    }
}