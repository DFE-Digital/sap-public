using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance;

public interface IAcademicPerformanceEnglishAndMathsResultsService
{
    public EnglishAndMathsResultsModel GetEnglishAndMathsResults(string urn, int selectedGrade);
}
