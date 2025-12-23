using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformancePupilProgressViewModel : SecondarySchoolBaseViewModel
{
    public required GcseDataViewModel GcseChartData { get; set; }
}
