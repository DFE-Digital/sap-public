using System.Reflection;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;

public sealed class FakeGenericRepository<T> : IGenericRepository<T> where T : class
{
    // Add any URNs you use in UI tests here
    private static readonly Dictionary<string, Establishment> Establishments = new(StringComparer.OrdinalIgnoreCase)
    {
        // From your CSV
        ["143034"] = new Establishment
        {
            URN = "143034",
            EstablishmentName = "St Paul's Church of England Academy",
            LAId = 845,
            LAName = "East Sussex",
            EstablishmentNumber = 3090,
            PhaseOfEducationId = 2,
            PhaseOfEducationName = "Primary"
        },

        ["100273"] = new Establishment
        {
            URN = "100273",
            EstablishmentName = "Saint Paul Roman Catholic Infant School",
            LAId = 204,
            LAName = "Hackney",
            EstablishmentNumber = 3658,
            PhaseOfEducationId = 2,
            PhaseOfEducationName = "Primary"
        },

        ["102848"] = new Establishment
        {
            URN = "102848",
            EstablishmentName = "SS Peter and Paul's Catholic Primary School",
            LAId = 317,
            LAName = "Redbridge",
            EstablishmentNumber = 3513,
            PhaseOfEducationId = 2,
            PhaseOfEducationName = "Primary"
        },

        // This was in your earlier logs; keep a fallback for it so UI tests don’t error
        ["105574"] = new Establishment
        {
            URN = "105574",
            EstablishmentName = "Test Secondary School",
            LAId = 999,
            LAName = "Test LA",
            EstablishmentNumber = 9999,
            PhaseOfEducationId = 4,
            PhaseOfEducationName = "Secondary"
        }
    };

    public Task<T?> ReadAsync(string id, CancellationToken ct = default)
        => ReadSingleAsync(new { Id = id }, ct);

    public Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default)
        => Task.FromResult(Enumerable.Empty<T>());

    public Task<T?> ReadSingleAsync(object? parameters, CancellationToken ct = default)
    {
        if (parameters is null) return Task.FromResult<T?>(default);

        // Establishment is looked up with ReadSingleAsync(new { Id = urn })
        if (typeof(T) == typeof(Establishment))
        {
            var id = GetPropertyString(parameters, "Id");
            if (!string.IsNullOrWhiteSpace(id) && Establishments.TryGetValue(id, out var est))
                return Task.FromResult(est as T);

            // Return a safe empty model so MVC doesn't throw
            return Task.FromResult(new Establishment { URN = id } as T);
        }

        // Other single-reads: return default (or you can add more “safe” objects)
        return Task.FromResult<T?>(default);
    }

    public Task<IEnumerable<T>> ReadManyAsync(object? parameters, CancellationToken ct = default)
    {
        if (parameters is null) return Task.FromResult(Enumerable.Empty<T>());

        // Subject entries page depends on EstablishmentSubjectEntryRow from v_establishment_subject_entries
        if (typeof(T) == typeof(EstablishmentSubjectEntryRow))
        {
            var urn = GetPropertyString(parameters, "Urn");
            if (string.IsNullOrWhiteSpace(urn))
                return Task.FromResult(Enumerable.Empty<T>());

            // Make sure you return enough rows to trigger:
            // - core + additional sections
            // - pagination (if your UI paginates at e.g. 10)
            // Cohort size (pupil_count) should be consistent and >0.
            var cohort = 100;

            var rows = new List<EstablishmentSubjectEntryRow>();

            // Core subjects (at least a few)
            rows.AddRange(MakeRows(urn, cohort, "English Language", "GCSE", 30));
            rows.AddRange(MakeRows(urn, cohort, "Mathematics", "GCSE", 35));
            rows.AddRange(MakeRows(urn, cohort, "Biology", "GCSE", 20));
            rows.AddRange(MakeRows(urn, cohort, "Chemistry", "GCSE", 18));
            rows.AddRange(MakeRows(urn, cohort, "Physics", "GCSE", 15));
            rows.AddRange(MakeRows(urn, cohort, "Combined Science", "GCSE", 40));
            rows.AddRange(MakeRows(urn, cohort, "Computer Science", "GCSE", 12));

            // Additional subjects (create many to force pagination)
            var additionalSubjects = new[]
            {
                "History", "Geography", "French", "Spanish", "Art", "Music", "Drama",
                "PE", "Business", "Design & Technology", "RS", "Media Studies",
                "Sociology", "Psychology", "Citizenship"
            };

            foreach (var s in additionalSubjects)
                rows.AddRange(MakeRows(urn, cohort, s, "GCSE", 8));

            return Task.FromResult(rows.Cast<T>());
        }

        return Task.FromResult(Enumerable.Empty<T>());
    }

    private static IEnumerable<EstablishmentSubjectEntryRow> MakeRows(
        string urn, int cohort, string subject, string qual, int count)
    {
        // Your aggregation groups by subject + qualification; grades are summed.
        // We'll just make a single “grade bucket” row per subject.
        return new[]
        {
            new EstablishmentSubjectEntryRow
            {
                school_urn = urn,
                pupil_count = cohort,
                subject = subject,
                qualification_type = qual,
                qualification_detailed = null,
                grade = "All",
                number_achieving = count
            }
        };
    }

    private static string? GetPropertyString(object obj, string name)
    {
        var prop = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        return prop?.GetValue(obj)?.ToString();
    }
}
