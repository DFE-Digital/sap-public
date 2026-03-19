using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using System.Text.RegularExpressions;

namespace SAPPub.Infrastructure.Repositories.KS4.SubjectEntries
{
    public sealed class EstablishmentSubjectEntriesRepository : IEstablishmentSubjectEntriesRepository
    {
        private readonly IGenericRepository<EstablishmentSubjectEntryRow> _repo;
        private const string TotalExamEntriesRowIndicator = "Total exam entries";

        public EstablishmentSubjectEntriesRepository(IGenericRepository<EstablishmentSubjectEntryRow> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<EstablishmentCoreSubjectEntries> GetCoreSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new EstablishmentCoreSubjectEntries { SubjectEntries = new List<EstablishmentCoreSubjectEntries.SubjectEntry>() };

            var rows = (await _repo.ReadManyAsync(new { Urn = urn }, ct))
                .Where(r => IsCoreSubject(r.subject) && r.grade == TotalExamEntriesRowIndicator);

            if (rows == null || !rows.Any())
                return new EstablishmentCoreSubjectEntries { SubjectEntries = new List<EstablishmentCoreSubjectEntries.SubjectEntry>() };

            var core = rows
                .Select(r => new EstablishmentCoreSubjectEntries.SubjectEntry
                {
                    SubEntCore_Sub_Est_Current_Num = r.subject_discount_group != null && r.subject_discount_group.Contains("Maths", StringComparison.OrdinalIgnoreCase)
                        ? Regex.Replace(r.subject_discount_group, "Maths", "Mathematics", RegexOptions.IgnoreCase).Trim()
                        : r.subject_discount_group?.Trim(),
                    SubEntCore_Qual_Est_Current_Num = r.qualification_type ?? r.qualification_detailed,
                    SubEntCore_Entr_Est_Current_Num = r.number_achieving
                })
                .OrderBy(r => r.SubEntCore_Sub_Est_Current_Num)
                .ThenBy(r => r.SubEntCore_Qual_Est_Current_Num)
                .ToList();

            return new EstablishmentCoreSubjectEntries { SubjectEntries = core };
        }

        public async Task<EstablishmentAdditionalSubjectEntries> GetAdditionalSubjectEntriesByUrnAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return new EstablishmentAdditionalSubjectEntries { SubjectEntries = new List<EstablishmentAdditionalSubjectEntries.SubjectEntry>() };

            var rows = (await _repo.ReadManyAsync(new { Urn = urn }, ct))
                .Where(r => !IsCoreSubject(r.subject) && r.grade == TotalExamEntriesRowIndicator);

            if (rows == null || !rows.Any())
                return new EstablishmentAdditionalSubjectEntries { SubjectEntries = new List<EstablishmentAdditionalSubjectEntries.SubjectEntry>() };

            var additional = rows
                .Select(r => new EstablishmentAdditionalSubjectEntries.SubjectEntry
                {
                    SubEntAdd_Sub_Est_Current_Num = r.subject_discount_group?.Trim(),
                    SubEntAdd_Qual_Est_Current_Num = (r.qualification_type ?? r.qualification_detailed)?.Trim(),
                    SubEntAdd_Entr_Est_Current_Num = r.number_achieving
                })
                .OrderBy(r => r.SubEntAdd_Sub_Est_Current_Num)
                .ThenBy(r => r.SubEntAdd_Qual_Est_Current_Num)
                .ToList();

            return new EstablishmentAdditionalSubjectEntries { SubjectEntries = additional };
        }

        private sealed record AggregatedSubjectRow(string Subject, string? Qualification, double TotalEnteredPercent);

        private static readonly HashSet<string> CoreSubjectCategories = new(StringComparer.OrdinalIgnoreCase)
        {
            "English Language",
            "English Literature",
            "Mathematics",
            "Mathematics AS level",
            "Statistics",
            "Biology",
            "Chemistry",
            "Physics",
            "Combined Science",
            "Other Sciences"
        };

        private static bool IsCoreSubject(string? subject)
            => !string.IsNullOrWhiteSpace(subject) && CoreSubjectCategories.Contains(subject.Trim());
    }
}
