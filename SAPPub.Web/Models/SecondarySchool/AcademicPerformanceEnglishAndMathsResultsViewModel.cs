using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Models.Charts;
using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceEnglishAndMathsResultsViewModel : SecondarySchoolBaseViewModel
{
    public enum GcseGradeDataSelection
    {
        [Display(Name = "Grade 4 and above")]
        Grade4AndAbove = 4,
        [Display(Name = "Grade 5 and above")]
        Grade5AndAbove = 5
    }

    public GcseGradeDataSelection? SelectedGrade { get; set; }

    public required GcseDataViewModel GcseChartData { get; set; }

    public static AcademicPerformanceEnglishAndMathsResultsViewModel Map(Establishment establishment, EnglishAndMathsResultsServiceModel? englishAndMathsResultsServiceModel, GcseGradeDataSelection selectedGrade)
    {
        var gcseData = new GcseDataViewModel
        {
            Labels = new List<string>
            {
                "School",
                string.IsNullOrEmpty(englishAndMathsResultsServiceModel?.LAName)
                    ? "Local Authority average"
                    : $"{englishAndMathsResultsServiceModel!.LAName} average",
                "England average"
            },
            GcseData = new List<double>
                {
                    englishAndMathsResultsServiceModel?.EstablishmentResult ?? 0,
                    englishAndMathsResultsServiceModel?.LocalAuthorityAverage ?? 0, // temp substitute nulls with 0 until we determine expected behaviour when data is unavailable
                    englishAndMathsResultsServiceModel?.EnglandAverage ?? 0
                },
            ChartTitle = $"GCSE English and Maths (Grade {selectedGrade} and above)",
        };
        return new AcademicPerformanceEnglishAndMathsResultsViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            SelectedGrade = selectedGrade,
            GcseChartData = gcseData
        };
    }
}
