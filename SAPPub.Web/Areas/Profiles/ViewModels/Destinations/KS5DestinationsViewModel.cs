using SAPPub.Core.ServiceModels.Destinations;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Destinations;

public class KS5DestinationsViewModel : SecondarySchoolBaseViewModel
{
    public required DataViewModel AllDestinationsData { get; set; }

    public required DisplayField<double> NumberOfStudentsIncludedInMeasure { get; set; }

    public required DisplayField<bool> HasEstablishmentData { get; set; }

    public static KS5DestinationsViewModel Map(KS5DestinationsDetails destinationsDetails)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(destinationsDetails.LocalAuthorityName);
        
        return new KS5DestinationsViewModel
        {
            URN = destinationsDetails.Urn,
            SchoolName = destinationsDetails.SchoolName,
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = true,
            NumberOfStudentsIncludedInMeasure = destinationsDetails.EstablishmentTotalCohortFor.ToDisplayField(),
            AllDestinationsData = new DataViewModel
            {
                Labels = ["School or College", laAverageLabel, "England average"],
                Data = [destinationsDetails.EstablishmentTotalOverall, destinationsDetails.LATotalOverall, destinationsDetails.EnglandOverall],
            },
            HasEstablishmentData = (destinationsDetails.EstablishmentTotalOverall is double && destinationsDetails.EstablishmentTotalOverall != 0).ToDisplayField()
        };
    }
}