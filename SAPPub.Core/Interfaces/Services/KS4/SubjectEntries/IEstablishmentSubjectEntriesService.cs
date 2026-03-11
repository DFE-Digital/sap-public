using SAPPub.Core.Entities.KS4.SubjectEntries;

namespace SAPPub.Core.Interfaces.Services.KS4.SubjectEntries
{
    public interface IEstablishmentSubjectEntriesService
    {
        Task<(EstablishmentCoreSubjectEntries Core, EstablishmentAdditionalSubjectEntries Additional)>
            GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
    }
}
