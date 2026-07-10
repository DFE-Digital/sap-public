using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceAdditionalMeasuresViewModel : SecondarySchoolBaseViewModel
{
    public required AdditionalMeasures EstablishmentCurrentYear { get; set; }
    public required AdditionalMeasures LocalAuthorityCurrentYear { get; set; }
    public required AdditionalMeasures EnglandCurrentYear { get; set; }

    public static AcademicPerformanceAdditionalMeasuresViewModel Map(
        AdditionalMeasuresModel additionalMeasuresModel)
    {
        return new AcademicPerformanceAdditionalMeasuresViewModel
        {
            URN = additionalMeasuresModel.Urn,
            SchoolName = additionalMeasuresModel.SchoolName,
            EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear,
            LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear,
            EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear
        };
    }
}