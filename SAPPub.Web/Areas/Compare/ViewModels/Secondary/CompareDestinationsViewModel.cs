using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareDestinationsViewModel : CompareSecondarySchoolBaseViewModel
{
    public class SchoolDestinationDetails
    {
        public required string URN { get; set; }
        public required string SchoolName { get; set; }
        public required DisplayField<string> SixthForm { get; set; }
        public required double? PercentInEducationEmploymentOrTraining { get; set; }

        public static SchoolDestinationDetails Map(DestinationsDetails destinationsDetails, Establishment establishmentDetails)
        {
            return new SchoolDestinationDetails
            {
                URN = destinationsDetails.Urn,
                SchoolName = destinationsDetails.SchoolName,
                SixthForm = GetSixthForm(establishmentDetails.OfficialSixthFormId).ToDisplayField(),
                PercentInEducationEmploymentOrTraining = destinationsDetails.SchoolAll.CurrentYear,
            };
        }

        private static string? GetSixthForm(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : string.Equals(value, "1") ? Yes : No;
        }
    }

    //public required List<string> URNs { get; set; } // from base class
    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public required double? EnglandPercentage { get; set; }

    // Ideally I think the data would sit in SchoolDestinationDetails and be transformed into the shape needed for the graph
    //public required DataViewModel AllDestinationsData { get; set; }

    public DataViewModel AllDestinationsData => new DataViewModel
    {
        Labels = SchoolDetails.Select(s => s.SchoolName).ToList().Concat(new[] { "England average" }).ToList(),
        Data = SchoolDetails.Select(s => s.PercentInEducationEmploymentOrTraining).ToList().Concat([EnglandPercentage]).ToList()
    };

    public required IEnumerable<SchoolDestinationDetails> SchoolDetails { get; set; }

    public static CompareDestinationsViewModel Map(List<string> urns, List<DestinationsDetails> destinationsDetails, List<Establishment> establishments)
    {
        return new CompareDestinationsViewModel
        {
            URNs = urns,
            SchoolDetails = destinationsDetails
                .Select(d => SchoolDestinationDetails.Map(d, establishments.First(e => e.URN == d.Urn))),
            EnglandPercentage = destinationsDetails.First().EnglandAll.CurrentYear,
            //HasEstablishmentData = new[]
            //    {
            //        destinationsDetails.SchoolAll.CurrentYear,
            //        destinationsDetails.SchoolAll.PreviousYear,
            //        destinationsDetails.SchoolAll.TwoYearsAgo,
            //    }.All(d => d is double v && v != 0),
            HasEstablishmentData = true.ToDisplayField(), // CML TODO
            //AllDestinationsData = new DataViewModel
            //{
            //    Labels = destinationsDetails
            //        .Select(d => d.SchoolName)
            //        .ToList()
            //        .Concat(["England average"])
            //        .ToList(),
            //    Data = destinationsDetails
            //        .Select(d => d.SchoolAll.CurrentYear)
            //        .ToList()
            //        .Concat([destinationsDetails.First().EnglandAll.CurrentYear])
            //        .ToList(),
            //},
        };
    }
}
