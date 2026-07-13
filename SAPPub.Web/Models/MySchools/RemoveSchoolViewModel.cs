using SAPPub.Core.ServiceModels;

namespace SAPPub.Web.Models.MySchools;

public class RemoveSchoolViewModel
{
    public required string Urn { get; set; }
    public string? Name { get; set; }

    public static RemoveSchoolViewModel MapFrom(EstablishmentServiceModel establishment)
    {
        return new RemoveSchoolViewModel
        {
            Urn = establishment.URN,
            Name = establishment.EstablishmentName
        };
    }
}
