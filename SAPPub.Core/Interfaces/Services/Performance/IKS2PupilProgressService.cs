using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS2PupilProgressService
{
    [Obsolete("GetPupilProgressAsync with this signature is deprecated, please use other GetPupilProgressAsync. Should not be used beyond Dec 10 2026")]
    Task<KS2PupilPerformance> GetPupilProgressAsync(string urn, AcademicYearSelection selectedYear, CancellationToken ct = default);

    Task<KS2PupilPerformanceBandings> GetPupilProgressAsync(string urn, CancellationToken ct = default);

    Task<KS2PupilPerformanceBandings> GetDisadvantagedPupilProgressAsync(string urn, CancellationToken ct = default);

    /// <summary>
    /// Gets Key Stage 2 pupil performance metrics for pupils with English as an additional language.
    /// </summary>
    /// <param name="urn">The unique reference number of the school.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Key Stage 2 pupil performance bandings.</returns>
    Task<KS2PupilPerformanceBandings> GetEalPupilProgressAsync(string urn, CancellationToken ct = default);

    /// <summary>
    /// Retrieves pupil progress performance bandings for girls and boys.
    /// </summary>
    /// <param name="urn">The unique reference number of the school.</param>
    /// <param name="ct">The token used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the pupil progress performance bandings for girls and boys.</returns>
    Task<KS2PupilPerformanceBandings[]> GetGirlBoyPupilProgressAsync(string urn, CancellationToken ct = default);
}
