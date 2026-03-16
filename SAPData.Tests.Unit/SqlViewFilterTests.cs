using SAPData;
using Xunit;

public class SqlViewFilterTests
{
    [Fact]
    public void ExcludeOnlineSchools_GeneratesExpectedSql()
    {
        var filter = new SqlViewFilter("ExcludeOnlineSchools", t => $"{t}.\"typeofestablishment__code_\" <> '49'");
        var sql = filter.GetSqlCondition("t");
        Assert.Equal("t.\"typeofestablishment__code_\" <> '49'", sql);
    }

    [Fact]
    public void ExcludeClosed3YrSchools_GeneratesExpectedSql()
    {
        var cutoff = "2022-09-12";
        var filter = new SqlViewFilter("ExcludeClosed3YrSchools", t =>
            $"{t}.\"closedate\" IS NULL OR {t}.\"closedate\" = '' OR TO_DATE({t}.\"closedate\", 'DD/MM/YYYY') >= '{cutoff}'");
        var sql = filter.GetSqlCondition("t");
        Assert.Equal("t.\"closedate\" IS NULL OR t.\"closedate\" = '' OR TO_DATE(t.\"closedate\", 'DD/MM/YYYY') >= '2022-09-12'", sql);
    }

    [Fact]
    public void IncludeKS4_GeneratesExpectedSql()
    {
        var filter = new SqlViewFilter("IncludeKS4", t => $"{t}.\"phaseofeducation__code_\" IN (4, 5, 7)");
        var sql = filter.GetSqlCondition("t");
        Assert.Equal("t.\"phaseofeducation__code_\" IN (4, 5, 7)", sql);
    }
}
