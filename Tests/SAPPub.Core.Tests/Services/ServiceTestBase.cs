using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services;

public abstract class ServiceTestBase
{
    public static CodedDouble GetCodedDouble(double val) => new(val, string.Empty, val.ToString());
}
