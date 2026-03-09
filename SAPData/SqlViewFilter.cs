using System;

namespace SAPData;

public class SqlViewFilter
{
    public string Name { get; }
    public Func<string, string> GetSqlCondition { get; }

    public SqlViewFilter(string name, Func<string, string> getSqlCondition)
    {
        Name = name;
        GetSqlCondition = getSqlCondition;
    }
}