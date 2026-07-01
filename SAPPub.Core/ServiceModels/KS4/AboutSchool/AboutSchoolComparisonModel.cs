namespace SAPPub.Core.ServiceModels.KS4.AboutSchool;

public class AboutSchoolComparisonModel
{
    public required string Urn { get; set; }

    public required string SchoolName { get; set; }

    public string? Website { get; set; }

    public string? Address { get; set; }

    public string? LocalAuthority { get; set; }

    public string? LocalAuthorityName { get; set; }

    public string? LocalAuthorityWebsite { get; set; }

    public string Easting { get; set; } = string.Empty;

    public string Northing { get; set; } = string.Empty;
}
