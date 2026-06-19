using SAPData.Filters;
using SAPData.Models;
using Xunit;

namespace SAPData.Unit.Tests;

public class GenerateViewsTests : IDisposable
{
    private readonly string _baseDir;
    private readonly string _sqlDir;
    private readonly string _mappingPath;

    public GenerateViewsTests()
    {
        _baseDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _sqlDir = Path.Combine(_baseDir, "Sql");
        Directory.CreateDirectory(_sqlDir);

        _mappingPath = Path.Combine(_sqlDir, "tablemapping.csv");
    }

    public void Dispose()
    {
        if (Directory.Exists(_baseDir))
            Directory.Delete(_baseDir, recursive: true);
    }

    // ------------------------------------------------------------
    // HELPERS
    // ------------------------------------------------------------

    private static DataMapRow Row(
        string file,
        string range,
        string type,
        string property,
        string field,
        string recordBy = "URN")
    {
        return new DataMapRow
        {
            FileName = file,
            Range = range,
            Type = type,
            PropertyName = property,
            Field = field,
            RecordFilterBy = recordBy
        };
    }

    private void WriteMapping(params (string key, string table)[] entries)
    {
        File.WriteAllLines(
            _mappingPath,
            entries.Select(e => $"{e.key},{e.table}")
        );
    }


    // ------------------------------------------------------------
    // ESTABLISHMENT VIEW
    // ------------------------------------------------------------

    [Fact]
    public void Generates_establishment_view_or_explains_why_it_was_skipped()
    {
        // Mapping alone is no longer sufficient; GenerateViews resolves dataset keys via raw_sources.json.
        WriteMapping(("edubasealldata", "t_edubase_123abcdef01"));

        var rows = new List<DataMapRow>();
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "04_v_establishment.sql");
        Assert.True(File.Exists(path));

        var sql = File.ReadAllText(path);

        // If the generator cannot resolve the key, it emits a "skipped" stub with a reason.
        Assert.Contains("v_establishment", sql);
        Assert.Contains("view SQL was skipped", sql);
        Assert.Contains("Could not resolve dataset key from raw_sources.json", sql);
    }

    [Fact]
    public void EstablishmentView_Includes_All_Required_Filters()
    {
        // Arrange
        WriteMapping(("edubasealldata20230912", "t_edubase_20230912"));
        var rows = new List<DataMapRow>();

        var filters = SqlViewFilterProvider.GetEstablishmentFilters();

        // Act
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_v_establishment.sql"));

        // Assert: check each filter's SQL is present
        foreach (var filter in filters)
        {
            var expectedSql = filter.GetSqlCondition("t");
            Assert.Contains(expectedSql, sql);
        }
        Assert.Contains("WHERE", sql);
    }

    [Fact]
    public void EstablishmentView_Includes_KS4_CTE_And_ISKS4_Column()
    {
        //Arrange
        WriteMapping(("edubasealldata20230912", "t_edubase_20230912"), ("ks4_perf", "t_ks4_perf"));
        var rows = new List<DataMapRow>
        {
            Row("ks4_perf", "Establishment", "KS4_Performance", "SomeProp", "some_field")
        };

        // Act
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();
        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_v_establishment.sql"));

        // Assert: CTE and ISKS4 logic present
        Assert.Contains("ks4_urns AS", sql);
        Assert.Contains("t.\"urn\" IN (SELECT \"urn\" FROM ks4_urns)", sql);
        Assert.DoesNotContain("ks5_urns AS", sql);
        Assert.DoesNotContain("t.\"urn\" IN (SELECT \"urn\" FROM ks5_urns)", sql);
        Assert.Contains("CASE WHEN", sql);
        Assert.Contains("AS \"ISKS4\"", sql);
    }

    [Fact]
    public void EstablishmentView_Includes_KS5_CTE_And_ISKS5_Column()
    {
        //Arrange
        WriteMapping(("edubasealldata20230912", "t_edubase_20230912"), ("ks5_perf", "t_ks5_perf"));
        var rows = new List<DataMapRow>
        {
            Row("ks5_perf", "Establishment", "KS5_Performance", "SomeProp", "some_field")
        };

        // Act
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();
        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_v_establishment.sql"));

        // Assert: CTE and ISKS5 logic present
        Assert.Contains("ks5_urns AS", sql);
        Assert.Contains("t.\"urn\" IN (SELECT \"urn\" FROM ks5_urns)", sql);
        Assert.DoesNotContain("ks4_urns AS", sql);
        Assert.DoesNotContain("t.\"urn\" IN (SELECT \"urn\" FROM ks4_urns)", sql);
        Assert.Contains("CASE WHEN", sql);
        Assert.Contains("AS \"ISKS5\"", sql);
    }

    [Fact]
    public void EstablishmentView_Filters_Include_KeyStage_Conditions()
    {
        // Arrange
        WriteMapping(("edubasealldata20230912", "t_edubase_20230912"), ("ks4_perf", "t_ks4_perf"));
        var rows = new List<DataMapRow>
    {
        Row("ks4_perf", "Establishment", "KS4_Performance", "SomeProp", "some_field")
    };

        var filters = SqlViewFilterProvider.GetEstablishmentFilters(["KS4", "KS5"],
            new Dictionary<string, string> { { "KS4", "t.\"urn\" IN (SELECT \"urn\" FROM ks4_urns)" } }
        );

        // Act
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();
        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_v_establishment.sql"));

        // Assert: Each filter's SQL is present
        foreach (var filter in filters)
        {
            var expectedSql = filter.GetSqlCondition("t");
            Assert.Contains(expectedSql, sql);
        }
    }

    [Fact]
    public void EstablishmentView_Includes_SenTypes_Column()
    {
        //Arrange
        WriteMapping(("edubasealldata20230912", "t_edubase_20230912"));
        var rows = new List<DataMapRow>
        {
            Row("edubasealldata20230912", "Establishment", "All establishment data", "SomeProp", "some_field")
        };

        // Act
        new GenerateViews(rows, _mappingPath, _sqlDir).Run();
        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_v_establishment.sql"));

        // Assert: CTE and ISKS4 logic present
        Assert.Contains("NULLIF(concat_ws(', ', NULLIF(t.\"sen1__name_\", 'Not Applicable')", sql);
        Assert.Contains("AS \"SenTypes\"", sql);
    }

    // ------------------------------------------------------------
    // FACT VIEWS
    // ------------------------------------------------------------

    [Fact]
    public void Generates_fact_view_when_rows_exist()
    {
        WriteMapping(("ks4_dest", "raw_ks4_dest_abc"));

        var rows = new List<DataMapRow>
        {
            Row("ks4_dest", "England", "KS4_Destinations", "Overall", "overall")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "04_v_england_destinations.sql");
        Assert.True(File.Exists(path));

        var sql = File.ReadAllText(path);
        Assert.Contains("CREATE MATERIALIZED VIEW v_england_destinations", sql);
        Assert.Contains("MAX(CASE WHEN TRUE THEN", sql);
        Assert.Contains("AS \"Overall\"", sql);
    }

    [Fact]
    public void Skips_view_when_no_matching_rows()
    {
        WriteMapping(("ks4_dest", "t_ks4_dest_abc1234567"));

        var rows = new List<DataMapRow>
        {
            // Different range/type → no output
            Row("ks4_dest", "LA", "KS4_Performance", "P8", "p8")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var path = Path.Combine(_sqlDir, "04_v_england_destinations.sql");

        // New behaviour: generator writes a stub file explaining the skip
        Assert.True(File.Exists(path));

        var sql = File.ReadAllText(path);
        Assert.Contains("v_england_destinations", sql);
        Assert.Contains("view SQL was skipped", sql);
    }


    // ------------------------------------------------------------
    // TABLE MAPPING SAFETY
    // ------------------------------------------------------------

    [Fact]
    public void Throws_if_table_mapping_missing()
    {
        WriteMapping(); // empty

        var rows = new List<DataMapRow>
        {
            Row("missing_file", "England", "KS4_Destinations", "Overall", "overall")
        };

        Action act = () => new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Contains("Missing table mapping", ex.Message);
    }

    // ------------------------------------------------------------
    // MULTI-SOURCE VIEW
    // ------------------------------------------------------------

    [Fact]
    public void Coalesces_properties_from_multiple_sources()
    {
        WriteMapping(
            ("edubasealldata", "raw_edubase_123"), // REQUIRED
            ("file1", "raw_1"),
            ("file2", "raw_2")
        );

        var rows = new List<DataMapRow>
        {
            Row("file1", "England", "KS4_Performance", "Att8", "att8"),
            Row("file2", "England", "KS4_Performance", "Att8", "att8")
        };

        new GenerateViews(rows, _mappingPath, _sqlDir).Run();

        var sql = File.ReadAllText(
            Path.Combine(_sqlDir, "04_v_england_performance.sql"));

        Assert.Contains("COALESCE(", sql);
        Assert.Contains("src_1.\"Att8\"", sql);
        Assert.Contains("src_2.\"Att8\"", sql);
    }
}
