using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS5EstablishmentSubjectEntriesService
{
    Task<IEnumerable<SubjectsEnteredModel>> GetSubjectEntriesByUrnAsync(string urn, QualificationType? qualificationType, CancellationToken ct = default);
}
