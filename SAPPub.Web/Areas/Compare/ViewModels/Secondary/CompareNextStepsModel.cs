using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareNextStepsModel
{
    public string? EstablishmentName { get; set; }

    public string? URN { get; set; }

    public required DisplayField<string> Website { get; set; }

    public required DisplayField<string> Telephone { get; set; }
}
