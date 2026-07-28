using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.Performance;
using System.Globalization;

namespace SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;

public sealed class EstablishmentSubjectEntriesRepository(IGenericRepository<EstablishmentSubjectEntryRow> repo) : IEstablishmentSubjectEntriesRepository
{
    private const string QualType_GCSE = "GCSE";
    private const string QualType_Vocational = "Vocational";
    private const string TotalExamEntriesRowIndicator = "Total exam entries";

    private readonly IGenericRepository<EstablishmentSubjectEntryRow> _repo = repo ?? throw new ArgumentNullException(nameof(repo));

    public async Task<IEnumerable<SubjectsEntered>> GetGcseSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        var gcseSubjectsEntered = await GetSubjectsEntered(urn, r => r.qualification_type == QualType_GCSE && r.grade == TotalExamEntriesRowIndicator, ct);

        foreach (var subjectEntered in gcseSubjectsEntered.Where(a => a!.Subject!.Contains("Maths", StringComparison.InvariantCultureIgnoreCase)))
        {
            subjectEntered.Subject = subjectEntered?.Subject?.Replace("Maths", "Mathematics", true, CultureInfo.InvariantCulture).Trim();
        }

        return gcseSubjectsEntered;
    }

    public async Task<IEnumerable<SubjectsEntered>> GetVocationalAwardSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        return await GetSubjectsEntered(urn, r => r.qualification_type == QualType_Vocational && r.grade == TotalExamEntriesRowIndicator, ct);
    }

    public async Task<IEnumerable<SubjectsEntered>> GetOtherSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
    {
        return await GetSubjectsEntered(urn, r => (r.qualification_type != QualType_Vocational && r.qualification_type != QualType_GCSE) && r.grade == TotalExamEntriesRowIndicator, ct);
    }

    private async Task<IEnumerable<SubjectsEntered>> GetSubjectsEntered(string urn, Func<EstablishmentSubjectEntryRow, bool> whereClause, CancellationToken ct = default)
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
            .Select(r => new SubjectsEntered
            {
                Subject = r.subject_discount_group?.Trim(),
                Qualification = r.qualification_type ?? r.qualification_detailed,
                TotalNumberOfEntries = r.number_achieving
            })
            .OrderBy(r => r.Subject)
            .ThenBy(r => r.Qualification)];
    }
}