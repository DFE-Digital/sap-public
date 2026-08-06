using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using System.Collections;
using System.Reflection;

namespace SAPPub.Web.Tests;

public sealed class FakeGenericRepository<T> : IGenericRepository<T> where T : class
{
    private static readonly Dictionary<string, Establishment> Establishments = new(StringComparer.OrdinalIgnoreCase)
    {
        ["143034"] = new Establishment
        {
            URN = "143034",
            EstablishmentName = "St Paul's Church of England Academy",
            LAId = "845",
            LAName = "East Sussex",
            EstablishmentNumber = "3090",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary",
            AddressPostcode = "TN37 6RT",
            Website = "http://www.test.co.uk/",
            Easting = "580573",
            Northing = "110137",
            IsKS2 = true,
            IsKS4 = false
        },
        ["100273"] = new Establishment
        {
            URN = "100273",
            EstablishmentName = "Saint Paul Roman Catholic Infant School",
            LAId = "204",
            LAName = "Hackney",
            EstablishmentNumber = "3658",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary",
            IsKS4 = false,
            IsKS2 = true
        },
        ["102848"] = new Establishment
        {
            URN = "102848",
            EstablishmentName = "SS Peter and Paul's Catholic Primary School",
            LAId = "317",
            LAName = "Redbridge",
            EstablishmentNumber = "3513",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary",
            AddressPostcode = "IG1 1SA",
            IsKS4 = false
        },
        ["105574"] = new Establishment
        {
            URN = "105574",
            EstablishmentName = "Loreto High School Chorlton",
            LAId = "999",
            LAName = "Test LA",
            EstablishmentNumber = "9999",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "M21 7SW",
            Easting = "382682",
            Northing = "392995",
            Website = "http://www.test.co.uk/",
            SenTypes = "VI - Visual Impairment, HI - Hearing Impairment",
            IsKS4 = true,
            IsKS5 = false
        },
        ["137552"] = new Establishment
        {
            URN = "137552",
            EstablishmentName = "Stewards Academy - Science Specialist, Harlow",
            LAId = "881",
            TrustName = "THE PASSMORES CO-OPERATIVE LEARNING COMMUNITY",
            EstablishmentNumber = "4343",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "CM18 7NQ",
            StatusCode = 2,
            ClosedDate = null,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false,
        },
        ["107564"] = new Establishment
        {
            URN = "107564",
            EstablishmentName = "Todmorden High School",
            LAId = "381",
            EstablishmentNumber = "4026",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "OL14 7DG",
            StatusCode = 2,
            ClosedDate = "23-03-2025",
            IsKS4 = true
        },
        ["145744"] = new Establishment // school with recent open date and having predecessors
        {
            URN = "145744",
            EstablishmentName = "Abbey Park School",
            LAId = "381",
            EstablishmentNumber = "123",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "SN25 2ND",
            OpenDate = "01-09-2025", // recent open date to test recently opened school message - this will fail in 2028
            IsKS4 = true
        },
        ["178965"] = new Establishment
        {
            URN = "178965",
            EstablishmentName = "Predecessor 1 to Abbey Park School",
            LAId = "381",
            EstablishmentNumber = "1234",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "SN25 2ND",
            StatusCode = 2,
            ClosedDate = "01-09-2025",
            IsKS4 = true
        },
        ["178966"] = new Establishment
        {
            URN = "178966",
            EstablishmentName = "Predecessor 2 to Abbey Park School",
            LAId = "381",
            EstablishmentNumber = "1235",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "SN25 2ND",
            StatusCode = 2,
            IsKS4 = true
        },
        ["151948"] = new Establishment
        {
            URN = "151948",
            EstablishmentName = "Abbeyfield School",
            LAId = "381",
            EstablishmentNumber = "1234",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "SN15 3XB",
            IsKS4 = true
        },
        ["100279"] = new Establishment
        {
            URN = "100279",
            EstablishmentName = "Stoke Newington School and Sixth Form",
            LAId = "381",
            EstablishmentNumber = "236",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "N16 9EX",
            IsKS4 = true
        },
        ["145179"] = new Establishment
        {
            URN = "145179",
            EstablishmentName = "Stoke Park School",
            LAId = "381",
            EstablishmentNumber = "895",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "CV2 4JW",
            IsKS4 = true
        },
        ["147059"] = new Establishment
        {
            URN = "147059",
            EstablishmentName = "Stone Lodge School",
            LAId = "381",
            EstablishmentNumber = "698",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "DA2 6FY",
            IsKS4 = true
        },
        ["145055"] = new Establishment
        {
            URN = "145055",
            EstablishmentName = "Stowmarket High School",
            LAId = "381",
            EstablishmentNumber = "456",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "IP14 1QR",
            IsKS4 = true
        },
        ["149893"] = new Establishment
        {
            URN = "149893",
            EstablishmentName = "The Abbey School",
            LAId = "381",
            EstablishmentNumber = "154",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "ME13 8RZ",
            IsKS4 = true
        },
        ["137020"] = new Establishment
        {
            URN = "137020",
            EstablishmentName = "West Hill School",
            LAId = "381",
            EstablishmentNumber = "7896",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "SK15 1LX",
            IsKS4 = true
        },
        ["149976"] = new Establishment
        {
            URN = "149976",
            EstablishmentName = "Four Elms Primary School",
            LAId = "886",
            EstablishmentNumber = "2134",
            PhaseOfEducationId = "2",
            PhaseOfEducationName = "Primary",
            AddressPostcode = "TN8 6NE",
            IsKS2 = true,
            IsKS4 = false,
            IsKS5 = false,
        },
        ["130499"] = new Establishment
        {
            URN = "130499",
            EstablishmentName = "Holy Cross College",
            LAId = "351",
            EstablishmentNumber = "8600",
            PhaseOfEducationId = "6",
            PhaseOfEducationName = "16 plus",
            AddressPostcode = "BL9 9BB",
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = true,
        },
        ["135600"] = new Establishment
        {
            URN = "135600",
            EstablishmentName = "Ark Academy",
            LAId = "304",
            EstablishmentNumber = "6906",
            PhaseOfEducationId = "7",
            PhaseOfEducationName = "All-through",
            AddressPostcode = "HA9 9JR",
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
        },
        ["149328"] = new Establishment
        {
            URN = "149328",
            EstablishmentName = "King Edward VI High School",
            LAId = "860",
            EstablishmentNumber = "4020",
            PhaseOfEducationId = "4",
            PhaseOfEducationName = "Secondary",
            AddressPostcode = "ST17 9YJ",
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = true,
        },
        ["150009"] = new Establishment
        {
            URN = "150009",
            EstablishmentName = "Abraham Moss Community School",
            LAId = "352",
            EstablishmentNumber = "4271",
            PhaseOfEducationId = "7",
            PhaseOfEducationName = "All-through",
            AddressPostcode = "M8 5UF",
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = false,
        }
    };

    public static List<Establishment> GetAllEstablishments()
    {
        return [.. Establishments.Values];
    }

    private static readonly Dictionary<string, EstablishmentPerformance> EstablishmentPerformances = new(StringComparer.OrdinalIgnoreCase)
    {
        ["105574"] = new EstablishmentPerformance
        {
            Id = "105574",
            Attainment8_Tot_Est_Current_Num = 10,
            Attainment8_Tot_Est_Previous_Num = 20,
            Attainment8_Tot_Est_Previous2_Num = null,
            Prog8_Tot_Est_Previous_Num = 0.1,
            Prog8_Tot_Est_Previous2_Num = null,
            EngMaths49_Tot_Est_Current_Pct = 50,
            EngMaths59_Tot_Est_Current_Pct = 60,
            EngMaths49_Tot_Est_Previous_Pct = 65,
            EngMaths59_Tot_Est_Previous_Pct = 70,
            EngMaths49_Tot_Est_Previous2_Pct = 75,
            EngMaths59_Tot_Est_Previous2_Pct = 80,
            // additional measures
            AnyQual_Tot_Est_Current_Pct_Coded = new Core.ValueObjects.CodedDouble(90, "", ""),
            TripSci_Tot_Est_Current_Pct_Coded = new Core.ValueObjects.CodedDouble(80, "", ""),
            More1FL_Tot_Est_Current_Pct_Coded = new Core.ValueObjects.CodedDouble(70, "", ""),
            ExamEntriesGSCE_Tot_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(151, "", ""),
            ExamEntriesKS4_Tot_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(100, "", ""),
            Pup_Tot_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(200, "", "")
        },
        ["137020"] = new EstablishmentPerformance
        {
            Id = "137020",
            Attainment8_Tot_Est_Current_Num = 20,
            Attainment8_Tot_Est_Previous_Num = 10,
            Attainment8_Tot_Est_Previous2_Num = null,
            Prog8_Tot_Est_Previous_Num = 0.5,
            Prog8_Tot_Est_Previous2_Num = null,
            EngMaths49_Tot_Est_Current_Pct = 70,
            EngMaths59_Tot_Est_Current_Pct = 50,
            EngMaths49_Tot_Est_Previous_Pct = 55,
            EngMaths59_Tot_Est_Previous_Pct = 60,
            EngMaths49_Tot_Est_Previous2_Pct = 55,
            EngMaths59_Tot_Est_Previous2_Pct = 70,
            // additional measures
            AnyQual_Tot_Est_Current_Pct_Coded = new Core.ValueObjects.CodedDouble(null, "", ""),
            TripSci_Tot_Est_Current_Pct_Coded = new Core.ValueObjects.CodedDouble(null, "", ""),
            More1FL_Tot_Est_Current_Pct_Coded = new Core.ValueObjects.CodedDouble(null, "", ""),
            ExamEntriesGSCE_Tot_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(null, "", ""),
            ExamEntriesKS4_Tot_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(null, "", ""),
            Pup_Tot_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(null, "", "")
        },
    };

    private static readonly Dictionary<string, KS4EstablishmentDestinations> EstablishmentDestinations = new(StringComparer.OrdinalIgnoreCase)
    {
        ["105574"] = new KS4EstablishmentDestinations
        {
            Id = "105574",
            AllDest_Tot_Est_Current_Pct = 50,
            AllDest_Tot_Est_Previous_Pct = 20,
            AllDest_Tot_Est_Previous2_Pct = 30,
            Education_Tot_Est_Current_Pct = 47,
            Employment_Tot_Est_Current_Pct = 2,
            Apprentice_Tot_Est_Current_Pct = 1,

        },
        ["100279"] = new KS4EstablishmentDestinations
        {
            Id = "100279",
            AllDest_Tot_Est_Current_Pct = 50,
            AllDest_Tot_Est_Previous_Pct = 20,
            AllDest_Tot_Est_Previous2_Pct = 30,
            Education_Tot_Est_Current_Pct = 47,
            Employment_Tot_Est_Current_Pct = 2,
            Apprentice_Tot_Est_Current_Pct = 1,
        },
        ["149328"] = new KS4EstablishmentDestinations
        {
            Id = "149328",
            AllDest_Tot_Est_Current_Pct = null,
            AllDest_Tot_Est_Previous_Pct = null,
            AllDest_Tot_Est_Previous2_Pct = null,
            Education_Tot_Est_Current_Pct = null,
            Employment_Tot_Est_Current_Pct = null,
            Apprentice_Tot_Est_Current_Pct = null,
        },
    };

    private static readonly Dictionary<string, KS4EnglandDestinations> EnglandDestinations = new(StringComparer.OrdinalIgnoreCase)
    {
        ["105574"] = new KS4EnglandDestinations
        {
            Id = "105574",
            AllDest_Tot_Eng_Current_Pct = 50
        },
        ["100279"] = new KS4EnglandDestinations
        {
            Id = "100279",
            AllDest_Tot_Eng_Current_Pct = 50
        },
        ["149328"] = new KS4EnglandDestinations
        {
            Id = "149328",
            AllDest_Tot_Eng_Current_Pct = null
        }
    };

    private static readonly Dictionary<string, KS5EstablishmentDestinations> KS5EstablishmentDestinations = new(StringComparer.OrdinalIgnoreCase)
    {
        ["105574"] = new KS5EstablishmentDestinations
        {
            TOT_OVERALLPER_Est_Current_Pct = 50,
            TOT_COHORT_Est_Current_Num = 1020
        }
    };

    private static readonly Dictionary<string, KS2EstablishmentPerformance> KS2EstablishmentPerformances = new(StringComparer.OrdinalIgnoreCase)
    {
        ["149976"] = new KS2EstablishmentPerformance
        {
            Id = "149976",
            READ_AVERAGE_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(1, "", "1"),
            READ_AVERAGE_Est_Previous_Num_Coded = new Core.ValueObjects.CodedDouble(2, "", "2"),
            READ_AVERAGE_Est_Previous2_Num_Coded = new Core.ValueObjects.CodedDouble(3, "", "3"),
        },
        ["143034"] = new KS2EstablishmentPerformance
        {
            Id = "143034",
            READ_AVERAGE_Est_Current_Num_Coded = new Core.ValueObjects.CodedDouble(null, "Not available", "c"),
            READ_AVERAGE_Est_Previous_Num_Coded = new Core.ValueObjects.CodedDouble(2.1, "", "2.1"),
            READ_AVERAGE_Est_Previous2_Num_Coded = new Core.ValueObjects.CodedDouble(3.1, "", "3.1"),
        },
    };

    private static readonly Dictionary<string, KS2LAPerformance> KS2LAPerformances = new(StringComparer.OrdinalIgnoreCase)
    {
        ["886"] = new KS2LAPerformance
        {
            Id = "886",
            READ_AVERAGE_LA_Current_Num_Coded = new Core.ValueObjects.CodedDouble(1, "", "1"),
            READ_AVERAGE_LA_Previous_Num_Coded = new Core.ValueObjects.CodedDouble(2, "", "2"),
            READ_AVERAGE_LA_Previous2_Num_Coded = new Core.ValueObjects.CodedDouble(3, "", "3"),
        },
        ["845"] = new KS2LAPerformance
        {
            Id = "845",
            READ_AVERAGE_LA_Current_Num_Coded = new Core.ValueObjects.CodedDouble(1.1, "", "1.1"),
            READ_AVERAGE_LA_Previous_Num_Coded = new Core.ValueObjects.CodedDouble(2.1, "", "2.1"),
            READ_AVERAGE_LA_Previous2_Num_Coded = new Core.ValueObjects.CodedDouble(3.1, "", "3.1"),
        },
    };

    private static readonly List<KS2EnglandPerformance> KS2EnglandPerformances =
    [
        new KS2EnglandPerformance{
            Id = "",
            READ_AVERAGE_Eng_Current_Num_Coded = new Core.ValueObjects.CodedDouble(1, "", "1"),
            READ_AVERAGE_Eng_Previous_Num_Coded = new Core.ValueObjects.CodedDouble(2, "", "2"),
            READ_AVERAGE_Eng_Previous2_Num_Coded = new Core.ValueObjects.CodedDouble(3, "", "3"),
        },
        new KS2EnglandPerformance{}
    ];

    public Task<T?> ReadAsync(string id, CancellationToken ct = default)
        => ReadSingleAsync(new { Id = id }, ct);

    public Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default)
    {
        if (typeof(T) == typeof(Establishment))
        {
            return Task.FromResult(Establishments.Values.Select(e => (T)(object)e));
        }
        else return Task.FromResult(Enumerable.Empty<T>());
    }

    public Task<T?> ReadSingleAsync(object? parameters, CancellationToken ct = default)
    {
        if (parameters is null) return Task.FromResult<T?>(default);

        if (typeof(T) == typeof(Establishment))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && Establishments.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);

            return Task.FromResult<T?>((T)(object)new Establishment { URN = id ?? string.Empty });
        }

        if (typeof(T) == typeof(EstablishmentPerformance))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && EstablishmentPerformances.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);
        }

        if (typeof(T) == typeof(KS2EstablishmentPerformance))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && KS2EstablishmentPerformances.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);
        }

        if (typeof(T) == typeof(KS2LAPerformance))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && KS2LAPerformances.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);
        }

        if (typeof(T) == typeof(KS2EnglandPerformance))
        {
            return Task.FromResult<T?>((T)(object)KS2EnglandPerformances[0]);
        }

        if (typeof(T) == typeof(KS4EstablishmentDestinations))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && EstablishmentDestinations.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);
        }

        if (typeof(T) == typeof(KS4EnglandDestinations))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && EnglandDestinations.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);
        }

        if (typeof(T) == typeof(KS5EstablishmentDestinations))
        {
            var id = GetPropertyString(parameters, "Id");

            if (!string.IsNullOrWhiteSpace(id) && KS5EstablishmentDestinations.TryGetValue(id, out var est))
                return Task.FromResult<T?>((T)(object)est);
        }

        return Task.FromResult<T?>(default);
    }

    public Task<IEnumerable<T>> ReadManyAsync(object? parameters, CancellationToken ct = default)
    {
        if (parameters is null) return Task.FromResult(Enumerable.Empty<T>());

        if (typeof(T) == typeof(Establishment))
        {
            var urns = GetPropertyAsStringArray(parameters, "Urns");

            if (urns == null)
                return Task.FromResult(Enumerable.Empty<T>());

            return Task.FromResult(Establishments.Values.Where(e => urns.Contains(e.URN)).Select(e => (T)(object)e));
        }

        if (typeof(T) == typeof(EstablishmentPerformance))
        {
            var ids = GetPropertyAsStringArray(parameters, "Ids");

            if (ids == null)
                return Task.FromResult(Enumerable.Empty<T>());

            return Task.FromResult(EstablishmentPerformances.Values.Where(e => ids.Contains(e.Id)).Select(e => (T)(object)e));
        }

        if (typeof(T) == typeof(KS4EstablishmentSubjectEntryRow))
        {
            var urn = GetPropertyString(parameters, "Urn");
            if (string.IsNullOrWhiteSpace(urn))
                return Task.FromResult(Enumerable.Empty<T>());

            // Must be consistent and >0 or your aggregation returns empty
            var cohort = "100";

            var rows = new List<KS4EstablishmentSubjectEntryRow>
        {
            // Core
            MakeRow(urn, cohort, "English Language", "GCSE", "30"),
            MakeRow(urn, cohort, "Mathematics", "GCSE", "35"),
            MakeRow(urn, cohort, "Combined Science", "GCSE", "40"),
            MakeRow(urn, cohort, "Computer Science", "GCSE", "12"),

            // A few more core
            MakeRow(urn, cohort, "Biology", "GCSE", "20"),
            MakeRow(urn, cohort, "Chemistry", "GCSE", "18"),
            MakeRow(urn, cohort, "Physics", "GCSE", "15"),
        };

            // Additional (enough to trigger pagination)
            var additionalSubjects = new[]
            {
            "History", "Geography", "French", "Spanish", "Art", "Music", "Drama",
            "PE", "Business", "Design & Technology", "RS", "Media Studies",
            "Sociology", "Psychology", "Citizenship"
        };

            rows.AddRange(additionalSubjects.Select(s => MakeRow(urn, cohort, s, "GCSE", "8")));

            return Task.FromResult(rows.Cast<T>());
        }

        if (typeof(T) == typeof(KS5EstablishmentSubjectEntryRow))
        {
            var urn = GetPropertyString(parameters, "Urn");
            if (string.IsNullOrWhiteSpace(urn))
                return Task.FromResult(Enumerable.Empty<T>());

            var rows = new List<KS5EstablishmentSubjectEntryRow>
            {
                MakeKS5Row(urn, "Maths", "A level Mathematics", "Level 3", "50", "A level"),
                MakeKS5Row(urn, "English", "A level English Literature", "Level 3", "45", "A level"),
                MakeKS5Row(urn, "Biology", "A level Biology", "Level 3", "40", "A level"),
                MakeKS5Row(urn, "Chemistry", "A level Chemistry", "Level 3", "35", "A level"),
                MakeKS5Row(urn, "History", "A level History", "Level 3", "30", "A level"),
                MakeKS5Row(urn, "Sport", "BTEC National Sport", "Level 3", "25", "Other Academic"),
                MakeKS5Row(urn, "IT", "Cambridge Technical IT", "Level 3", "20", "Tech level"),
                MakeKS5Row(urn, "Health", "BTEC Health and Social Care", "Level 3", "15", "Technical certificate"),
            };

            return Task.FromResult(rows.Cast<T>());
        }

        return Task.FromResult(Enumerable.Empty<T>());
    }

    private KS5EstablishmentSubjectEntryRow MakeKS5Row(string urn, string subject, string qualDetailed, string qualLevel, string count, string examCohort) 
        => new()
        { 
            subject = subject,
            entries_count = count,
            qualification_detailed = qualDetailed,
            qualification_level = qualLevel,
            exam_cohort = examCohort,
            grade = "Total exam entries"
        };

    private static KS4EstablishmentSubjectEntryRow MakeRow(string urn, string cohort, string subject, string qual, string count)
        => new()
        {
            school_urn = urn,
            pupil_count = cohort,
            subject = subject,
            qualification_type = qual,
            qualification_detailed = null,
            grade = "All",
            number_achieving = count
        };

    private static string? GetPropertyString(object obj, string name)
    {
        var prop = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        return prop?.GetValue(obj)?.ToString();
    }


    private static string[]? GetPropertyAsStringArray(object obj, string name)
    {
        var prop = obj.GetType().GetProperty(name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        var value = prop?.GetValue(obj);

        if (value == null)
            return null;

        // If it's already a collection (array, list, etc.)
        if (value is IEnumerable enumerable && value is not string)
        {
            return enumerable.Cast<object?>()
                             .Select(x => x?.ToString() ?? string.Empty)
                             .ToArray();
        }

        // Single value -> wrap into array
        return [value.ToString() ?? string.Empty];
    }


    public Task<IEnumerable<T>> ReadPageAsync(int page, int take, CancellationToken ct = default)
    {
        // temporary implementation of pagination for fake data - just return empty for page > 1
        // to avoid complications of implementing actual pagination logic
        if (page > 1) return Task.FromResult(Enumerable.Empty<T>());
        var results = ReadAllAsync(ct).Result;
        return Task.FromResult(results.Take(take));
    }

    Task<bool> IGenericRepository<T>.WriteAsync(object? writeObject, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    Task<bool> IGenericRepository<T>.UpdateAsync(object? updateObject, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
