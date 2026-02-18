using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;

namespace SAPPub.Core.Services.KS4.SubjectEntries
{
    public sealed class EstablishmentSubjectEntriesService : IEstablishmentSubjectEntriesService
    {
        private readonly IEstablishmentSubjectEntriesRepository _repo;

        public EstablishmentSubjectEntriesService(IEstablishmentSubjectEntriesRepository subjectEntriesRepository)
        {
            _repo = subjectEntriesRepository ?? throw new ArgumentNullException(nameof(subjectEntriesRepository));
        }

        public async Task<(EstablishmentCoreSubjectEntries Core, EstablishmentAdditionalSubjectEntries Additional)>
            GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
        {
            var coreTask = _repo.GetCoreSubjectEntriesByUrnAsync(urn, ct);
            var additionalTask = _repo.GetAdditionalSubjectEntriesByUrnAsync(urn, ct);

            await Task.WhenAll(coreTask, additionalTask);

            return (await coreTask, await additionalTask);
        }
    }
}
