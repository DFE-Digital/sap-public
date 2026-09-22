namespace SAPPub.Web.Models.Config;

public class UrlLinksOptions
{
    public UrlLinkOptions PrimarySchoolAccountability { get; set; } = new();

    public UrlLinkOptions SecondarySchoolAccountabilityPriorAttainment { get; set; } = new();
}
