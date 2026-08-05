using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.ServiceModels.Performance;
using System.Globalization;

namespace SAPPub.Infrastructure.Repositories.Performance;

public sealed class KS4EstablishmentSubjectEntriesRepository(IGenericRepository<KS4EstablishmentSubjectEntryRow> repo) : IKS4EstablishmentSubjectEntriesRepository
{
    private const string QualType_GCSE = "GCSE";
    private const string QualType_Vocational = "Vocational";
    private const string TotalExamEntriesRowIndicator = "Total exam entries";

    private readonly IGenericRepository<KS4EstablishmentSubjectEntryRow> _repo = repo ?? throw new ArgumentNullException(nameof(repo));

    public async Task<IEnumerable<SubjectsEnteredModel>> GetGcseSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        var gcseSubjectsEntered = await GetSubjectsEntered(urn, r => r.qualification_type == QualType_GCSE && r.grade == TotalExamEntriesRowIndicator, ct);

        foreach (var subjectEntered in gcseSubjectsEntered.Where(a => a!.Subject!.Contains("Maths", StringComparison.InvariantCultureIgnoreCase)))
        {
            subjectEntered.Subject = subjectEntered?.Subject?.Replace("Maths", "Mathematics", true, CultureInfo.InvariantCulture).Trim();
        }

        return gcseSubjectsEntered;
    }

    public async Task<IEnumerable<SubjectsEnteredModel>> GetVocationalAwardSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        return await GetSubjectsEntered(urn, r => r.qualification_type == QualType_Vocational && r.grade == TotalExamEntriesRowIndicator, ct);
    }

    public async Task<IEnumerable<SubjectsEnteredModel>> GetOtherSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        return await GetSubjectsEntered(urn, r => (r.qualification_type != QualType_Vocational && r.qualification_type != QualType_GCSE) && r.grade == TotalExamEntriesRowIndicator, ct);
    }

    private async Task<IEnumerable<SubjectsEnteredModel>> GetSubjectsEntered(string urn, Func<KS4EstablishmentSubjectEntryRow, bool> whereClause, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
        {
            return [];
        }

        var rows = (await _repo.ReadManyAsync(new { Urn = urn }, ct)).Where(whereClause);

        if (rows is null || !rows.Any())
        {
            return [];
        }

        return [.. rows
            .Select(r => new SubjectsEnteredModel
            {
                Subject = r.subject_discount_group?.Trim(),
                Qualification = r.qualification_type ?? r.qualification_detailed,
                TotalNumberOfEntries = r.number_achieving
            })
            .OrderBy(r => r.Subject)
            .ThenBy(r => r.Qualification)];
    }
}