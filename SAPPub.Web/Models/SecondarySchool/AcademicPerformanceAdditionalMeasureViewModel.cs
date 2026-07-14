using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceAdditionalMeasureViewModel
{
    public required string MeasureName { get; set; }
    public required string MeasureFormat { get; set; } // percent, average, int
    public required DisplayField<CodedDouble> EstablishmentCurrentYear { get; set; }
    public required DisplayField<CodedDouble> LocalAuthorityCurrentYear { get; set; }
    public required DisplayField<CodedDouble> EnglandCurrentYear { get; set; }
}
