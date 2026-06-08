using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.MySchools;

public class MySchoolModel
{
    public required string Urn { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public EstablishmentStatus? Status { get; set; }
    public required DisplayField<DateOnly> ClosedDate { get; set; }

    public static MySchoolModel MapFrom(Establishment establishment)
    {
        return new MySchoolModel
        {
            Urn = establishment.URN,
            Name = establishment.EstablishmentName,
            Address = establishment.Address,
            Status = establishment.StatusCode.ToStatus(),
            ClosedDate = establishment.ClosedDate.ToDateOnly().ToDisplayField()
        };
    }
}
