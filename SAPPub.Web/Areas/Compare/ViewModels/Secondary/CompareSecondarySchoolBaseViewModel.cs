using SAPPub.Core.Enums;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareSecondarySchoolBaseViewModel
{
    private static string CurrentAcademicYearDisplayName => Ks4AcademicYearHelper.GetDisplayName(AcademicYearSelection.Current);
    public string? AcademicYearInfoParagraph => $"Information in this section is for the {CurrentAcademicYearDisplayName} academic year";

    public required List<string> URNs { get; set; } = [];

    public required bool ListContainsSpecialSchool { get; set; }
    public required bool ListContainsNonSpecialSchool { get; set; }

    public string RouteQueryString => URNs.Count == 0 ? string.Empty : $"?{string.Join("&", URNs.Select(urn => $"{RouteConstants.URNs}={urn}"))}";
}
