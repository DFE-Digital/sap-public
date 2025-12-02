using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceViewModel : SecondarySchoolBaseViewModel
{
    public required GcseDataViewModel GcseChartData { get; set; }
}
