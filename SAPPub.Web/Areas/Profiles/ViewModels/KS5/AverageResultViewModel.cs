using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class AverageResultViewModel
{
    public required RelativeYearValues<string> AcademicYears { get; init; }

    public required RelativeYearValues<DisplayField<CodedDouble>> NumberOfStudents { get; init; }

    public required RelativeYearValues<PerformanceResultViewModel> Establishment { get; init; }

    public required RelativeYearValues<PerformanceResultViewModel> LocalAuthority { get; init; }

    public required RelativeYearValues<PerformanceResultViewModel> England { get; init; }

    public static AverageResultViewModel Map(AverageResultModel model)
    {
        return new AverageResultViewModel
        {
            AcademicYears = new RelativeYearValues<string>
            {
                CurrentYear = CurrentYear,
                PreviousYear = PreviousYear,
                TwoYearsAgo = TwoYearsAgo,
            },
            NumberOfStudents = new RelativeYearValues<DisplayField<CodedDouble>>
            {
                CurrentYear = model.NumberOfStudents.CurrentYear.ToDisplayField(),
                PreviousYear = model.NumberOfStudents.PreviousYear.ToDisplayField(),
                TwoYearsAgo = model.NumberOfStudents.TwoYearsAgo.ToDisplayField(),
            },
            Establishment = new RelativeYearValues<PerformanceResultViewModel>
            { 
                CurrentYear = PerformanceResultViewModel.Map(model.Establishment.CurrentYear),
                PreviousYear = PerformanceResultViewModel.Map(model.Establishment.PreviousYear),
                TwoYearsAgo = PerformanceResultViewModel.Map(model.Establishment.TwoYearsAgo),
            },
            LocalAuthority = new RelativeYearValues<PerformanceResultViewModel>
            {
                CurrentYear = PerformanceResultViewModel.Map(model.LocalAuthority.CurrentYear),
                PreviousYear = PerformanceResultViewModel.Map(model.LocalAuthority.PreviousYear),
                TwoYearsAgo = PerformanceResultViewModel.Map(model.LocalAuthority.TwoYearsAgo),
            },
            England = new RelativeYearValues<PerformanceResultViewModel>
            {
                CurrentYear = PerformanceResultViewModel.Map(model.England.CurrentYear),
                PreviousYear = PerformanceResultViewModel.Map(model.England.PreviousYear),
                TwoYearsAgo = PerformanceResultViewModel.Map(model.England.TwoYearsAgo),
            },
        };
    }
}
