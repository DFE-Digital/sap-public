using SAPPub.Core.ServiceModels;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareNextStepsViewModel : CompareSecondarySchoolBaseViewModel
{
    public IEnumerable<CompareNextStepsModel> SchoolDetailList { get; set; } = [];

    public static CompareNextStepsViewModel Map(List<string> urns, IEnumerable<EstablishmentServiceModel> establishments)
    {
        return new CompareNextStepsViewModel
        {
            URNs = urns,

            SchoolDetailList = establishments
                .OrderBy(x => x.EstablishmentName)
                .Select(a => new CompareNextStepsModel
                {
                    EstablishmentName = a.EstablishmentName,
                    Website = a.Website.ToDisplayField(),
                    Telephone = a.TelephoneNum.ToDisplayField(),
                    URN = a.URN
                })
        };
    }
}
