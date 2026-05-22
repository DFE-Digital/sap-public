using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Models.SecondarySchool;

public class SuccessorOrPredecessorDetailsModel
{
    public required string Urn { get; set; }
    public required string Name { get; set; }

    public static SuccessorOrPredecessorDetailsModel Map(EstablishmentLinkModel establishmentLink)
    {
        return new SuccessorOrPredecessorDetailsModel
        {
            Urn = establishmentLink.Urn,
            Name = establishmentLink.Name
        };
    }
}