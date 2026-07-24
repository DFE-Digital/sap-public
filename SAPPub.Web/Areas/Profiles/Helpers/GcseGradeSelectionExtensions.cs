using SAPPub.Core.Enums;

namespace SAPPub.Web.Areas.Profiles.Helpers;
public static class GcseGradeSelectionExtensions
{
    public static string ToRouteSegment(this GcseGradeDataSelection grade)
    {
        return grade switch
        {
            GcseGradeDataSelection.Grade4AndAbove => "grade-4-and-above",
            GcseGradeDataSelection.Grade5AndAbove => "grade-5-and-above",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public static GcseGradeDataSelection FromRouteSegment(string routeSegment)
    {
        return routeSegment switch
        {
            "grade-4-and-above" => GcseGradeDataSelection.Grade4AndAbove,
            "grade-5-and-above" => GcseGradeDataSelection.Grade5AndAbove,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static int ToGradeValue(this GcseGradeDataSelection grade)
    {
        return Convert.ToInt32(grade);
    }

    public record GcseGradeDataSelectionOption
    {
        public GcseGradeDataSelection Grade { get; set; }

        public string RouteSegmentName => Grade switch
        {
            GcseGradeDataSelection.Grade4AndAbove => "grade-4-and-above",
            GcseGradeDataSelection.Grade5AndAbove => "grade-5-and-above",
            _ => throw new ArgumentOutOfRangeException()
        };

        public GcseGradeDataSelectionOption(GcseGradeDataSelection Grade)
        {
            this.Grade = Grade;
        }
    }
}
