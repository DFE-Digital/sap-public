using System.Text.RegularExpressions;
using Xunit;

namespace SAPData.Tests.Unit;

public class GenerateIndexesTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _sqlDir;
    private readonly string _sqlPath;
    private readonly string _sql;

    public GenerateIndexesTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        _sqlDir = Path.Combine(_tempDir, "Sql");
        Directory.CreateDirectory(_sqlDir);

        new GenerateIndexes(_sqlDir).Run();

        _sqlPath = Path.Combine(_sqlDir, "04_indexes.sql");
        _sql = File.ReadAllText(_sqlPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    [Fact]
    public void Generates_indexes_sql_file()
    {
        Assert.True(File.Exists(_sqlPath));
    }

    [Fact]
    public void Writes_expected_header()
    {
        Assert.Contains("-- 04_indexes.sql", _sql);
        Assert.Contains("Indexes for materialized views", _sql);
        Assert.Contains("AUTO-GENERATED", _sql);
    }

    [Fact]
    public void Uses_current_schema_so_it_works_in_any_schema()
    {
        Assert.Contains("current_schema()", _sql);
        Assert.Contains("v_schema text := current_schema()", _sql);
    }

    [Fact]
    public void Guards_index_creation_when_view_is_missing()
    {
        // ensures the DO blocks are checking if the view exists before attempting to create an index
        Assert.Contains("IF to_regclass(format('%I.%I', v_schema", _sql);
        Assert.Contains("RAISE NOTICE 'SKIP: view", _sql);
    }

    [Fact]
    public void Generates_index_logic_for_each_expected_view()
    {
        // These views should appear in the to_regclass checks
        Assert.Contains("'v_england_destinations'", _sql);
        Assert.Contains("'v_england_performance'", _sql);
        Assert.Contains("'v_establishment'", _sql);
        Assert.Contains("'v_establishment_absence'", _sql);
        Assert.Contains("'v_establishment_destinations'", _sql);
        Assert.Contains("'v_establishment_performance'", _sql);
        Assert.Contains("'v_establishment_workforce'", _sql);
        Assert.Contains("'v_la_destinations'", _sql);
        Assert.Contains("'v_la_performance'", _sql);
    }

    [Fact]
    public void Uses_urn_for_establishment_index()
    {
        Assert.Contains("idx_v_establishment_urn", _sql);

        // In the EXECUTE format string, the column is emitted as ("URN")
        Assert.Matches(
            new Regex(@"CREATE INDEX %I ON %I\.%I\s*\(""URN""\)", RegexOptions.Multiline),
            _sql
        );
    }

    [Fact]
    public void Uses_Id_for_expected_non_establishment_views()
    {
        Assert.Contains("idx_v_england_destinations_id", _sql);
        Assert.Contains("idx_v_la_performance_id", _sql);

        Assert.Matches(
            new Regex(@"idx_v_england_destinations_id.*\(""Id""\)", RegexOptions.Singleline),
            _sql
        );

        Assert.Matches(
            new Regex(@"idx_v_la_performance_id.*\(""Id""\)", RegexOptions.Singleline),
            _sql
        );
    }

    [Fact]
    public void Does_not_use_public_schema_literal()
    {
        // Old generator used "public.v_*". New generator should not hardcode schema.
        Assert.DoesNotContain("public.v_", _sql);
        Assert.DoesNotContain("ON public.", _sql);
    }

    [Fact]
    public void Implements_idempotency_via_to_regclass_checks()
    {
        // New approach: check if index exists via to_regclass rather than CREATE INDEX IF NOT EXISTS
        Assert.Contains("to_regclass(format('%I.%I', v_schema, 'idx_", _sql);
        Assert.Contains("RAISE NOTICE 'OK: index", _sql);
    }
}
