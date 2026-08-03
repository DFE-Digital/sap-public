using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels
{
    public class SimpleCodedDoubleTableViewModel
    {
        public required DisplayField<CodedDouble> SchoolOrCollege { get; init; }

        public required DisplayField<CodedDouble> LocalAuthority { get; init; }

        public required DisplayField<CodedDouble> England { get; init; }

        public static SimpleCodedDoubleTableViewModel Map(SimpleCodedDoubleTableModel model)
        {
            return new SimpleCodedDoubleTableViewModel
            {
                England = model.England.ToDisplayField(),
                LocalAuthority = model.LocalAuthority.ToDisplayField(),
                SchoolOrCollege = model.SchoolOrCollege.ToDisplayField()
            };
        }
    }
}
