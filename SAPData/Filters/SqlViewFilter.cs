namespace SAPData.Filters;

public class SqlViewFilter(string name, Func<string, string> getSqlCondition)
{
    public string Name { get; } = name;
    public Func<string, string> GetSqlCondition { get; } = getSqlCondition;
}