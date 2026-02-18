namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public interface IReasonCodeLookup
{
    bool TryGet(string code, out string reasonText);
}
