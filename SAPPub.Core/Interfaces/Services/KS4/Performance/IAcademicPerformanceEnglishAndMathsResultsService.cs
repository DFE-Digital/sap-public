using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance
{
    public interface IAcademicPerformanceEnglishAndMathsResultsService
    {
        Task<EnglishAndMathsResultsModel> GetEnglishAndMathsResultsAsync(string urn, int selectedGrade, CancellationToken ct = default);
    }
}
