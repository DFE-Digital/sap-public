using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class SchoolAttainmentDetailsViewModel
{
    public required string URN { get; set; }
    public required string SchoolName { get; set; }
    public required double? Attainment8Score { get; set; }
    public required DisplayField<string> Attainment8ScoreContextDescription { get; set; }

    public static SchoolAttainmentDetailsViewModel Map(SchoolAttainmentAndProgressDetails attainmentDetails, EstablishmentServiceModel establishmentDetails)
    {
        var establishmentAttainment8ContextSentence = CommonHelper.EstablishmentAttainment8ContextStatement(attainmentDetails.Attainment8Score);
        
        return new SchoolAttainmentDetailsViewModel
        {
            URN = attainmentDetails.Urn,
            SchoolName = establishmentDetails.EstablishmentName,
            Attainment8Score = attainmentDetails.Attainment8Score,
            Attainment8ScoreContextDescription = establishmentAttainment8ContextSentence != null
                ? $"Pupils generally scored the equivalent of {establishmentAttainment8ContextSentence} in their 8 best GCSE-level subjects.".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
        };
    }
}
