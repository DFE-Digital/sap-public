namespace SAPPub.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DbColumnNameAttribute : Attribute
{
    private readonly string columnName;

    public DbColumnNameAttribute(string columnName)
    {
        this.columnName = columnName;
    }

    public string ColumnName => columnName;
}
