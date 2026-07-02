using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareAboutYourSchoolViewModel
{
    public required string Urn { get; set; }

    public required string SchoolName { get; set; }

    public string? Website { get; set; }

    public required DisplayField<string> Address { get; set; }

    public string? LocalAuthority { get; set; }

    public string? LocalAuthorityName { get; set; }

    public string? LocalAuthorityWebsite { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
