using SAPPub.Core.ServiceModels;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformanceAdditionalMeasuresViewModel : BaseViewModel
{
    public static AcademicPerformanceAdditionalMeasuresViewModel Map(EstablishmentServiceModel establishment)
    {
        return new AcademicPerformanceAdditionalMeasuresViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5
        };
    }
}