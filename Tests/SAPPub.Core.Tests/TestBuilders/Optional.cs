namespace SAPPub.Core.Tests.TestBuilders;

public sealed class Optional<T>
{
    public bool IsSet { get; private set; }
    public T? Value { get; private set; }

    public void SetValue(T? value)
    {
        Value = value;
        IsSet = true;
    }
}
