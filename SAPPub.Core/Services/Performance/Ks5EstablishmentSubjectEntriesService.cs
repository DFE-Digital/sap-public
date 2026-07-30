using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.Performance;

public sealed class KS5EstablishmentSubjectEntriesService(IKS5EstablishmentSubjectEntriesRepository subjectEntriesRepository) : IKS5EstablishmentSubjectEntriesService
{
    private readonly IKS5EstablishmentSubjectEntriesRepository _repo = subjectEntriesRepository ?? throw new ArgumentNullException(nameof(subjectEntriesRepository));

    private readonly string[] AcademicQualifications = ["A level", "Other academic"];
    private readonly string[] VocationalQualifications = ["Applied general", "Tech level", "Technical certificate"];

    async Task<IEnumerable<SubjectsEnteredModel>> IKS5EstablishmentSubjectEntriesService.GetSubjectEntriesByUrnAsync(string urn, QualificationType? qualificationType, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var results = await _repo.GetSubjectEntriesByUrnAsync(urn, ct);

        return qualificationType switch
        {
            QualificationType.AcademicQualifications => results.Where(a => AcademicQualifications.Contains(a.ExamCohort)),
            QualificationType.VocationalAndTechnicalQualifications => results.Where(a => VocationalQualifications.Contains(a.ExamCohort)),
            _ => results
        };
    }
}