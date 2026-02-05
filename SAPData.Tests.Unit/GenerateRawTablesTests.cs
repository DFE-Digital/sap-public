using System.Text;
using Xunit;

namespace SAPData.Tests.Unit;

public class GenerateRawTablesTests : IDisposable
{
    private readonly string _root;
    private readonly string _input;
    private readonly string _clean;
    private readonly string _sql;

    public GenerateRawTablesTests()
    {
        _root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _input = Path.Combine(_root, "input");
        _clean = Path.Combine(_root, "clean");
        _sql = Path.Combine(_root, "sql");

        Directory.CreateDirectory(_input);
        Directory.CreateDirectory(_clean);
        Directory.CreateDirectory(_sql);
    }

    public void Dispose()
    {
        if (Directory.Exists(_root))
            Directory.Delete(_root, recursive: true);
    }

    // -------------------------------------------------------
    // HELPERS
    // -------------------------------------------------------
    private void WriteCsv(string name, string content)
    {
        File.WriteAllText(
            Path.Combine(_input, name + ".csv"),
            content,
            new UTF8Encoding(false));
    }

    // -------------------------------------------------------
    // TESTS
    // -------------------------------------------------------

    [Fact]
    public void Generates_all_expected_sql_files()
    {
        WriteCsv("test-dataset", "a,b\n1,2");

        new GenerateRawTables(_input, _clean, _sql).Run();

        Assert.True(File.Exists(Path.Combine(_sql, "01_create_raw_tables.sql")));
        Assert.True(File.Exists(Path.Combine(_sql, "02_copy_into_raw.sql")));
        Assert.True(File.Exists(Path.Combine(_sql, "02_copy_into_raw_local.sql")));
        Assert.True(File.Exists(Path.Combine(_sql, "tablemapping.csv")));
    }

    [Fact]
    public void Generates_short_table_names_with_hash()
    {
        WriteCsv(
            "this-is-a-very-very-long-dataset-name-that-would-break-postgres",
            "a\n1");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var mapping = File.ReadAllText(Path.Combine(_sql, "tablemapping.csv"));

        // mapping format is: dataset_key,table_name
        // table_name format observed: t_<sanitised_prefix>_<10hexhash>
        Assert.Matches(@"^.+,t_[a-z0-9_]+_[a-f0-9]{10}\s*$", mapping);
    }



    [Fact]
    public void Table_name_is_deterministic()
    {
        WriteCsv("dataset", "a\n1");

        new GenerateRawTables(_input, _clean, _sql).Run();
        var first = File.ReadAllText(Path.Combine(_sql, "tablemapping.csv"));

        Directory.Delete(_sql, true);
        Directory.CreateDirectory(_sql);

        new GenerateRawTables(_input, _clean, _sql).Run();
        var second = File.ReadAllText(Path.Combine(_sql, "tablemapping.csv"));

        Assert.Equal(first, second);
    }

    [Fact]
    public void Pads_missing_columns()
    {
        WriteCsv(
            "padtest",
            "a,b,c\n1,2");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var cleaned = File.ReadAllLines(Path.Combine(_clean, "padtest.clean.csv"));
        Assert.Equal("1,2,", cleaned[1]);
    }

    [Fact]
    public void Truncates_extra_columns()
    {
        WriteCsv(
            "trunctest",
            "a,b\n1,2,3,4");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var cleaned = File.ReadAllLines(Path.Combine(_clean, "trunctest.clean.csv"));
        Assert.Equal("1,2", cleaned[1]);
    }

    [Fact]
    public void Sanitises_column_names()
    {
        WriteCsv(
            "sanitize",
            "LA-code,School-Name\n1,X");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var sql = File.ReadAllText(Path.Combine(_sql, "01_create_raw_tables.sql"));

        Assert.Matches("(?i)\"la_code\"", sql);
        Assert.Matches("(?i)\"school_name\"", sql);
    }


    [Fact]
    public void Writes_clean_csv_without_bom()
    {
        WriteCsv("bomtest", "a\n1");

        new GenerateRawTables(_input, _clean, _sql).Run();

        var bytes = File.ReadAllBytes(Path.Combine(_clean, "bomtest.clean.csv"));

        Assert.NotEqual(new byte[] { 0xEF, 0xBB, 0xBF }, bytes.Take(3).ToArray());
    }
}
