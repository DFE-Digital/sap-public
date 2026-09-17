using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public static class CodedDoubleFactory
{
    public static CodedDouble Create(double? value = null)
    {
        if (value != null)
            return new CodedDouble(value, "", value?.ToString() ?? "");
        else return new CodedDouble(null, "Not available", "z");
    }
}
