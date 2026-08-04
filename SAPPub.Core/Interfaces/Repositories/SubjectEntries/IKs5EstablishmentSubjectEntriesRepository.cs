using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Repositories.SubjectEntries;

public interface IKS5EstablishmentSubjectEntriesRepository
{
    Task<IEnumerable<SubjectsEnteredModel>> GetSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default);
}
