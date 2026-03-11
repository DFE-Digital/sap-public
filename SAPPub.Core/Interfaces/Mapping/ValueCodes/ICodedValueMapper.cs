namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public interface ICodedValueMapper
{
    void Apply<T>(IEnumerable<T> items) where T : class;
    void Apply<T>(T item) where T : class;
}