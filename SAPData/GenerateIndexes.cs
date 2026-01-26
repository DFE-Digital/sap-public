using System.Text;

namespace SAPData;

public class GenerateIndexes
{
    private readonly string _sqlDir;

    public GenerateIndexes(string sqlDir)
    {
        _sqlDir = sqlDir;
    }

    public void Run()
    {
        string outputPath = Path.Combine(_sqlDir, "04_indexes.sql");

        var sb = new StringBuilder();

        sb.AppendLine("-- ================================================================");
        sb.AppendLine("-- 04_indexes.sql");
        sb.AppendLine("-- Indexes for materialized views (AUTO-GENERATED)");
        sb.AppendLine("-- ================================================================");
        sb.AppendLine();
        sb.AppendLine("-- NOTE:");
        sb.AppendLine("-- - Uses quoted identifiers to respect case-sensitive columns");
        sb.AppendLine("-- - Guards index creation when a view is not present");
        sb.AppendLine();

        // View → column mapping (EXACT column names)
        var indexes = new Dictionary<string, string>
        {
            // Establishment
            { "v_establishment",                 "\"URN\"" },
            { "v_establishment_links",           "\"URN\"" },
            { "v_establishment_group_links",     "\"URN\"" },
            { "v_establishment_subject_entries", "\"school_urn\"" },

            { "v_establishment_absence",      "\"Id\"" },
            { "v_establishment_destinations", "\"Id\"" },
            { "v_establishment_performance",  "\"Id\"" },
            { "v_establishment_workforce",    "\"Id\"" },

            // England
            { "v_england_destinations", "\"Id\"" },
            { "v_england_performance",  "\"Id\"" },

            // Local Authority
            { "v_la_destinations", "\"Id\"" },
            { "v_la_performance",  "\"Id\"" }
        };

        foreach (var kvp in indexes)
        {
            string view = kvp.Key;
            string column = kvp.Value;

            string indexName = $"idx_{view}_{column.Trim('"').ToLowerInvariant().Replace(" ", "_")}";

            // Only create the index if the view exists (avoids pipeline failure when some views are skipped)
            sb.AppendLine("DO $$");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"  IF to_regclass('public.{view}') IS NOT NULL THEN");
            sb.AppendLine($"    CREATE INDEX IF NOT EXISTS {indexName}");
            sb.AppendLine($"    ON public.{view} ({column});");
            sb.AppendLine("  END IF;");
            sb.AppendLine("END $$;");
            sb.AppendLine();
        }

        File.WriteAllText(outputPath, sb.ToString(), new UTF8Encoding(false));

        Console.WriteLine("Generated view index script:");
        Console.WriteLine(outputPath);
    }
}
