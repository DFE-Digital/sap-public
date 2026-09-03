using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public static class CodedDoubleFactory
{
    public static CodedDouble Create(double value)
    {
        return new CodedDouble(value, string.Empty, value.ToString());
    }
}
