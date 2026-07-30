using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5
{
    public class EnglishMathsQualificationsDisadvantagedViewModel
    {
        public required DisplayField<CodedDouble> SchoolOrCollege { get; init; }

        public required DisplayField<CodedDouble> LocalAuthority { get; init; }

        public required DisplayField<CodedDouble> England { get; init; }

        public static EnglishMathsQualificationsDisadvantagedViewModel Map(EnglishMathsQualificationsDisadvantagedModel model)
        {
            return new EnglishMathsQualificationsDisadvantagedViewModel
            {
                England = model.England.ToDisplayField(),
                LocalAuthority = model.LocalAuthority.ToDisplayField(),
                SchoolOrCollege = model.SchoolOrCollege.ToDisplayField()
            };
        }
    }
}
