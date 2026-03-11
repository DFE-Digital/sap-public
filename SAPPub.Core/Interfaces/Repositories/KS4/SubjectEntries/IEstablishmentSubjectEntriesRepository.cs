using SAPPub.Core.Entities.KS4.SubjectEntries;

namespace SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries
{
    public interface IEstablishmentSubjectEntriesRepository
    {
        Task<EstablishmentCoreSubjectEntries> GetCoreSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
        Task<EstablishmentAdditionalSubjectEntries> GetAdditionalSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    }
}
