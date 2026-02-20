namespace SAPPub.Core.Interfaces.Repositories.Generic
{
    public interface IGenericRepository<T>
    {
        Task<T?> ReadAsync(string id, CancellationToken ct = default);
        Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default);
        Task<T?> ReadSingleAsync(object parameters, CancellationToken ct = default);
        Task<IEnumerable<T>> ReadManyAsync(object parameters, CancellationToken ct = default);
        Task<IEnumerable<T>> ReadPageAsync(int page, int take, CancellationToken ct = default);
    }
}
